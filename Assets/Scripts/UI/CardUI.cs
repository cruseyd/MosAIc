using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
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
    public static CardUI Spawn(Card card)
    {
        var cardUI = Spawn(card.data, card.agent);
        cardUI.SetVisible(false);
        cardUI.Define(card);
        return cardUI;
    }
    public static CardUI Spawn(CardData data, int agentID)
    {
        GameObject gameObject = null;
        if (data.prefab != null)
        {
            gameObject = Instantiate(data.prefab);
        } else {
            gameObject = Instantiate(ResourceManager.GetCardPrefab(data.type));
        }
        var cardUI = gameObject.GetComponent<CardUI>();
        cardUI.SetVisible(true);
        Debug.Assert(cardUI != null);
        cardUI.Define(data, agentID);
        return cardUI;
    }
    private void Define(Card card)
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
    private void Define(CardData data, int agentID)
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
}
