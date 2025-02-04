using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IDoubleClickable, IRightClickable
{
    public Card card { get; private set; }
    public TextMeshProUGUI nameText;
    public int agentID;
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
    public void Define(Card card)
    {
        nameText.text = card.data.name;
        agentID = card.agent;
        foreach (var statValue in card.data.baseStats)
        {
            StatName statName = statValue.stat;
            this.GetStat(statName).Initialize(card.GetStat(statName));
        }
        for (int textIndex = 0; textIndex < textRegions.Count; textIndex++)
        {
            textRegions[textIndex].text = card.data.GetText(textIndex);
        }
        if (this.card != null)
        {
            // ???
        } else {

        }
        this.card = card;
    }
    public void Define(CardData data, int agentID)
    {
        nameText.text = data.name;
        this.agentID = agentID;
        foreach (var statValue in data.baseStats)
        {
            StatName statName = statValue.stat;
            this.GetStat(statName).Initialize(data.GetBaseStatValue(statName));
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
    }
    public void OnDoubleClick(PointerEventData eventData)
    {
        Debug.Log("Double clicked Card with in zone " + card.zone.name.ToString() + " with position " + card.zonePosition.ToString());
        GameManager.HandleDoubleClick(this, eventData);
    }
    public void OnRightClick(PointerEventData eventData)
    {
        Debug.Log("Right clicked Card with in zone " + card.zone.name.ToString() + " with position " + card.zonePosition.ToString());
        GameManager.HandleRightClick(this, eventData);
    }
}
