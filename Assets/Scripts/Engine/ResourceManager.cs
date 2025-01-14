using System;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : PersistentSingleton<ResourceManager>
{
    private Dictionary<string, CardData> cardData = new Dictionary<string, CardData>();
    private Dictionary<CardType, GameObject> cardPrefabs = new Dictionary<CardType, GameObject>();
    private bool _initialized = false;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }
    private void Initialize()
    {
        if (!_initialized)
        {
            LoadCardData();
            LoadCardPrefabs();
            _initialized = true;
        }
    }
    private void LoadCardData()
    {
        cardData.Clear();
        var resources = Resources.LoadAll<CardData>("Cards");
        foreach (var data in resources)
        {
            Debug.Assert(!cardData.ContainsKey(data.name));
            cardData[data.name] = data;
        }
    }
    public static CardData GetCardData(string id)
    {
        instance.Initialize();
        Debug.Assert(instance.cardData.ContainsKey(id));
        return instance.cardData[id];
    }
    public static CardData GetRandomCardData()
    {
        instance.Initialize();
        var cards = instance.cardData.Keys;
        int roll = UnityEngine.Random.Range(0,cards.Count);
        int index = 0;
        foreach (var data in instance.cardData.Values)
        {
            if (index == roll) { return data; }
            index++;
        }
        Debug.LogError("ResourceManager.GetRandomCardData | Error: Failed to retrieve a card");
        return null;
    }
    private void LoadCardPrefabs()
    {
        cardPrefabs.Clear();
        foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
        {
            if (cardType == CardType.Default) { continue; }
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{cardType.ToString()}");
            if (prefab == null)
            {
                Debug.LogError($"ResourceManager.LoadCardPrefabs | Error: Could not find prefab {cardType.ToString()}");
            }
            cardPrefabs[cardType] = prefab;
        }

    }
    public static GameObject GetCardPrefab(CardType cardType)
    {
        instance.Initialize();
        return instance.cardPrefabs[cardType];
    }

}
