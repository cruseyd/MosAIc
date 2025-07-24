using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class DeckUI : CardZoneUI
{
    public CardZoneName sourceZone;
    public TextMeshProUGUI cardCountText;
    public override IEnumerator DoOrganize(float dt)
    {
        UpdateCardCount(zone.Cards().Count);
        yield return null;
    }

    private void UpdateCardCount(int value)
    {
        cardCountText.text = value.ToString();
    }
    private void Awake()
    {
        
    }

}
