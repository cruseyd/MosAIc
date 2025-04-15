using System;
using System.Collections;
using UnityEngine;

public class CardStackUI : CardZoneUI
{
    public override IEnumerator DoOrganize(float dt)
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
            var x = 0;
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
        animating = false;
    }
}
