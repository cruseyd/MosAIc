using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : PersistentSingleton<ResourceManager>
{
    private Dictionary<string, CardData> cardData = new Dictionary<string, CardData>();
    private Dictionary<CardType, List<CardData>> cardsByType = new Dictionary<CardType, List<CardData>>();
    private Dictionary<string, AgentData> agentData = new Dictionary<string, AgentData>();
    private Dictionary<AgentType, List<AgentData>> agentsByType = new Dictionary<AgentType, List<AgentData>>();
    private Dictionary<CardType, GameObject> cardPrefabs = new Dictionary<CardType, GameObject>();
    private Dictionary<AgentType, GameObject> agentPrefabs = new Dictionary<AgentType, GameObject>();
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
            LoadPrefabs();
            LoadCardData();
            LoadAgentData();
            _initialized = true;
        }
    }
    private void LoadCardData()
    {
        cardData.Clear();
        cardsByType.Clear();
        foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
        {
            cardsByType[cardType] = new List<CardData>();
        }
        var resources = Resources.LoadAll<CardData>("Cards");
        foreach (var data in resources)
        {
            Debug.Assert(!cardData.ContainsKey(data.name));
            cardData[data.name] = data;
            cardsByType[data.type].Add(data);
        }
    }
    private void LoadAgentData()
    {
        agentData.Clear();
        agentsByType.Clear();
        foreach (AgentType agentType in Enum.GetValues(typeof(AgentType)))
        {
            agentsByType[agentType] = new List<AgentData>();
        }
        var resources = Resources.LoadAll<AgentData>("Agents");
        foreach (var data in resources)
        {
            Debug.Assert(!cardData.ContainsKey(data.name));
            agentData[data.name] = data;
            agentsByType[data.type].Add(data);
        }
    }
    private static T GetRandom<T>(List<T> items)
    {
        return items[UnityEngine.Random.Range(0,items.Count)];
    }
    public static CardData GetCardData(string id)
    {
        instance.Initialize();
        Debug.Assert(instance.cardData.ContainsKey(id));
        return instance.cardData[id];
    }
    public static AgentData GetAgentData(string name)
    {
        instance.Initialize();
        Debug.Assert(instance.agentData.ContainsKey(name));
        return instance.agentData[name];
    }
    public static CardData GetRandomCardData(CardType type = CardType.Default)
    {
        instance.Initialize();
        if (type == CardType.Default)
        {
            return GetRandom(instance.cardData.Values.ToList());
        } else {
            return GetRandom(instance.cardsByType[type]);
        }
    }
    public static AgentData GetRandomAgentData(AgentType type = AgentType.Default)
    {
        instance.Initialize();
        if (type == AgentType.Default)
        {
            return GetRandom(instance.agentData.Values.ToList());
        } else {
            return GetRandom(instance.agentsByType[type]);
        }
    }
    
    private void LoadPrefabs()
    {
        cardPrefabs.Clear();
        agentPrefabs.Clear();
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach (GameObject prefab in prefabs)
        {
            var cardUI = prefab.GetComponent<CardUI>();
            if (cardUI != null)
            {
                cardPrefabs[cardUI.type] = prefab;
            }
            var agentUI = prefab.GetComponent<AgentUI>();
            if (agentUI != null)
            {
                agentPrefabs[agentUI.type] = prefab;
            }
        }
    }
    // private void LoadCardPrefabs()
    // {
    //     cardPrefabs.Clear();
    //     foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
    //     {
    //         if (cardType == CardType.Default) { continue; }
    //         GameObject prefab = Resources.Load<GameObject>($"Prefabs/{cardType.ToString()}");
    //         if (prefab == null)
    //         {
    //             Debug.LogError($"ResourceManager.LoadCardPrefabs | Error: Could not find prefab {cardType.ToString()}");
    //         }
    //         cardPrefabs[cardType] = prefab;
    //     }
    // }
    // private void LoadAgentPrefabs()
    // {
    //     agentPrefabs.Clear();
    //     foreach (AgentType agentType in Enum.GetValues(typeof(AgentType)))
    //     {
    //         if (agentType == AgentType.Default) { continue; }
    //         GameObject prefab = Resources.Load<GameObject>($"Prefabs/{agentType.ToString()}");
    //         if (prefab == null)
    //         {
    //             Debug.LogError($"ResourceManager.LoadAgentPrefabs | Error: Could not find prefab {agentType.ToString()}");
    //         }
    //         agentPrefabs[agentType] = prefab;
    //     }
    // }
    public static GameObject GetCardPrefab(CardType cardType)
    {
        instance.Initialize();
        Debug.Assert(cardType != CardType.Default);
        return instance.cardPrefabs[cardType];
    }
    public static GameObject GetAgentPrefab(AgentType agentType)
    {
        instance.Initialize();
        Debug.Assert(agentType != AgentType.Default);
        return instance.agentPrefabs[agentType];
    }
}
