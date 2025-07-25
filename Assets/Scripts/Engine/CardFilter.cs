using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class CardFilter
{
    public CardZoneName inZone = CardZoneName.Default;
    public int playerID = -1;
    public CardType isType = CardType.Default;

    public CardFilter() { }

    public bool Compare(CardData data)
    {
        if (isType != CardType.Default && data.type != isType) { return false; }
        return true;
    }
    public bool Compare(Card card)
    {
        if (inZone != CardZoneName.Default && card.zone.name != inZone) { return false; }
        if (playerID >= 0 && card.zone.player != playerID) { return false; }
        return Compare(card.data);
    }
}