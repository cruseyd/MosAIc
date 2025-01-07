using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ResourceManager : PersistentSingleton<ResourceManager>
{
    private Dictionary<string, CardData> cardData;

    protected override void Awake()
    {
        base.Awake();
        LoadCardData();
    }
    private void LoadCardData()
    {
        cardData = new Dictionary<string, CardData>();
        var resources = Resources.LoadAll<CardData>("Cards");
        foreach (var data in resources)
        {
            Debug.Assert(!cardData.ContainsKey(data.name));
            cardData[data.name] = data;
        }
    }
    public static CardData GetCardData(string id)
    {
        Debug.Assert(instance.cardData.ContainsKey(id));
        return instance.cardData[id];
    }
    public static CardData GetRandomCardData()
    {
        var cards = instance.cardData.Keys;
        int roll = Random.Range(0,cards.Count);
        int index = 0;
        foreach (var data in instance.cardData.Values)
        {
            if (index == roll) { return data; }
            index++;
        }
        Debug.LogError("ResourceManager.GetRandomCardData | Error: Failed to retrieve a card");
        return null;
    }
}
