using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoneUI : MonoBehaviour
{
    [SerializeField] public CardZoneID id;
    [SerializeField] public CardZone zone;
    [SerializeField] public float overlapSpacing;
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
    public bool animating { get; set;}
    public List<CardUI> GetCards()
    {
        List<CardUI> cards = new List<CardUI>();
        foreach (CardUI item in transform.GetComponentsInChildren<CardUI>())
        {
            cards.Add(item);
        }
        Debug.Assert(zone != null);
        cards.Sort((a,b) => zone.Compare(a.card.id, b.card.id));
        return cards;
    }
    protected int NumCards()
    {
        return transform.GetComponentsInChildren<CardUI>().Length;
    }
    
    public void Organize(float dt) {
        StartCoroutine(DoOrganize(dt));
    }
    public virtual IEnumerator DoOrganize(float dt)
    {
        
        if (animating) { yield return null; }
        var rect = GetComponent<RectTransform>();

        int N = NumCards();
        float spacing = this.width / N;
        float leftSideOffset = -rect.sizeDelta.x * rect.pivot.x;
        var cardList = GetCards();

        Vector2[] endPositions = new Vector2[N];
        Vector2[] startPositions = new Vector2[N];
        for (int n = 0; n < N; n++)
        {
            var c = cardList[n];
            var zoneIndex = zone.GetIndex(c.card.id);
            var s = Math.Min(zoneIndex.x, n);
            var x = spacing / 2.0f + s*spacing + leftSideOffset;
            var y = -overlapSpacing*zoneIndex.z;
            endPositions[n] = new Vector2(x, y);
            startPositions[n] = c.transform.localPosition;
        }
        float t = 0.0f;
        while (t < dt)
        {
            for (int n = 0; n < N; n++)
            {
                cardList[n].GetComponent<RectTransform>().pivot = Vector2.one*0.5f;
                cardList[n].transform.localPosition = Vector2.Lerp(startPositions[n], endPositions[n], t/dt);
            }
            t += Time.deltaTime;
            yield return null;
        }
        for (int n = 0; n < N; n++)
        {
            cardList[n].transform.localPosition = endPositions[n];
        }
        animating = false;
    }
    private void Awake()
    {
        animating = false;
    }

}
