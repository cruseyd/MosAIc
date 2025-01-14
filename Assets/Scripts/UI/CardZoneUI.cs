using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardZoneUI : MonoBehaviour
{
    public CardZoneName zoneName;
    public int agentID;
    private CardZone zone;
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
    public void Define(CardZone zone)
    {
        Debug.Assert(zone.agent == agentID);
        Debug.Assert(zone.name == zoneName);
        if (this.zone != null)
        {
            // ???
        }
        this.zone = zone;
    }
    private List<CardUI> GetCards()
    {
        List<CardUI> cards = new List<CardUI>();
        foreach (CardUI item in transform.GetComponentsInChildren<CardUI>())
        {
            cards.Add(item);
        }
        cards.Sort((a,b) => zone.Compare(a.card, b.card));
        return cards;
    }
    private int NumCards()
    {
        return transform.GetComponentsInChildren<CardUI>().Length;
    }
    private void OrganizeWithEvenSpacing()
    {
        int N = NumCards();
        float spacing = this.width / N;
        float position = spacing / 2.0f;
        foreach (var card in GetCards())
        {
            card.transform.localPosition = new Vector2(position, 0f);
            position += spacing;
        }
    }
    private void Awake()
    {

    }

}
