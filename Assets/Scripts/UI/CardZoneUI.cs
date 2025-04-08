using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class CardZoneUI : MonoBehaviour
{
    public CardZoneName zoneName;
    public int agentID;
    public CardZone zone;
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
    public bool animating { get; private set;}
    private List<CardUI> GetCards()
    {
        List<CardUI> cards = new List<CardUI>();
        foreach (CardUI item in transform.GetComponentsInChildren<CardUI>())
        {
            cards.Add(item);
        }
        Debug.Assert(zone != null);
        cards.Sort((a,b) => zone.Compare(a.card, b.card));
        return cards;
    }
    private int NumCards()
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

        Vector2[] endPositions = new Vector2[N];
        for (int n = 0; n < N; n++)
        {
            endPositions[n] = new Vector2(spacing / 2.0f + n*spacing + leftSideOffset, 0);
        }
        var cardList = GetCards();
        Vector2[] startPositions = new Vector2[N];
        for (int n = 0; n < N; n++)
        {
            startPositions[n] = cardList[n].transform.localPosition;
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
        animating = false;
    }
    private void Awake()
    {
        animating = false;
    }

}
