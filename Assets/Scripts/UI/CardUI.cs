using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Animations;

public class CardUI : MonoBehaviour, IDoubleClickable, IRightClickable, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovered = false;
    private bool doHoverZoom = false;
    private bool isVisible = true;
    private Vector3 baseScale = Vector3.one;
    private Canvas sortingCanvas;
    private int baseSortingOrder;
    public Vector3 targetScale = Vector3.one*2.0f;
    public CardType type;
    public Card card {get; set; }
    public long id;
    public TextMeshProUGUI nameText;
    public List<StatUI> stats = new List<StatUI>();
    public List<TextMeshProUGUI> textRegions = new List<TextMeshProUGUI>();
    public Transform front;
    public Transform back;
    public float width 
    {
        get {
            var rect = GetComponent<RectTransform>();
            Debug.Assert(rect != null);
            return rect.rect.width;
        }
    }
    public float height
    {
        get {
            var rect = GetComponent<RectTransform>();
            Debug.Assert(rect != null);
            return rect.rect.height;
        }
    }
    public void Define(CardData data)
    {
        Debug.Assert(data.type == type);
        nameText.text = data.name;
        foreach (var statValue in data.baseStats)
        {
            StatName statName = statValue.stat;
            GetStat(statName).statValue = data.GetBaseStatValue(statName);
        }
        for (int textIndex = 0; textIndex < textRegions.Count; textIndex++)
        {
            textRegions[textIndex].text = data.GetText(textIndex);
        }
    }
    private StatUI GetStat(StatName statName)
    {
        foreach (var stat in stats )
        {
            if (stat.statName == statName)
            {
                return stat;
            }
        }
        Debug.LogError($"CardUI.GetStat | Error: Could not find StatUI with StatName {statName.ToString()}");
        return null;
    }
    public void SetVisible(bool flag)
    {
        front.gameObject.SetActive(flag);
        back.gameObject.SetActive(!flag);
        isVisible = flag;
    }
    public void OnDoubleClick(PointerEventData eventData)
    {
        GameManager.HandleDoubleClick(this, eventData);
    }
    public void OnRightClick(PointerEventData eventData)
    {
        GameManager.HandleRightClick(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameStateUI.animating && isVisible && doHoverZoom)
        {
            isHovered = true;
            baseSortingOrder = sortingCanvas.sortingOrder;
            sortingCanvas.sortingOrder = 100;
            AdjustPivotByScreenQuadrant();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        sortingCanvas.sortingOrder = baseSortingOrder;
    }
    private void AdjustPivotByScreenQuadrant()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 oldWorldPos = rectTransform.position; // Store current world position
        Vector2 oldPivot = rectTransform.pivot; // Store current pivot

        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        Vector3 bottomLeft = worldCorners[0];
        Vector3 topRight = worldCorners[2];

        Vector3 cardCenter = (bottomLeft + topRight) / 2f;

        float pivotX = 0.5f, pivotY = 0.5f;

        pivotX = cardCenter.x / Screen.width;
        pivotY = cardCenter.y / Screen.height;

        // Convert old pivot to local position
        Vector2 pivotOffset = new Vector2(
            (pivotX - oldPivot.x) * rectTransform.rect.width * rectTransform.localScale.x,
            (pivotY - oldPivot.y) * rectTransform.rect.height * rectTransform.localScale.y
        );

        Vector3 positionOffset = rectTransform.TransformVector(pivotOffset);

        // Apply new pivot and offset position to compensate
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        rectTransform.position = oldWorldPos + positionOffset; 
    }
    private void zoneUpdate()
    {
        CardZoneUI zone = transform.parent.GetComponent<CardZoneUI>();
        doHoverZoom = !(zone is DeckUI || zone is CardStackUI);

        if (zone)
        {
            baseScale = zone.height / height * Vector3.one;
        } else {
            baseScale = Vector3.one;
        }
    }
    public void Awake()
    {
        sortingCanvas = GetComponent<Canvas>();
        if (sortingCanvas == null)
        {
            sortingCanvas = gameObject.AddComponent<Canvas>();
        }
        sortingCanvas.overrideSorting = true;
        if (gameObject.GetComponent<UnityEngine.UI.GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

    }
    public void Update()
    {
        if (GameStateUI.animating)
        {
            isHovered = false;
        }
        else {
            zoneUpdate();
        }
        
        Vector3 desiredScale = isHovered ? targetScale : baseScale;
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10f);
        
        foreach (var statUI in stats)
        {
            var statName = statUI.statName;
            statUI.statValue = card.GetStat(statName);
        }
    }
}
