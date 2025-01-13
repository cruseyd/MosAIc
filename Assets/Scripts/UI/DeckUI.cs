using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckUI : MonoBehaviour
{
    public CardZoneName zoneName;
    public int agentID;
    private Deck deck;
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

}
