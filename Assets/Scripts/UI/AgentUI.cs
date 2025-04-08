using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class AgentUI : MonoBehaviour
{
    public Agent agent { get; set; }
    public AgentType type;
    public int id;
    public TextMeshProUGUI nameText;
    public List<StatUI> stats = new List<StatUI>();
    public void Define(Agent newAgent)
    {
        nameText.text = newAgent.type.ToString() + " " + newAgent.ID;
        foreach (var statValue in agent.data.baseStats)
        {
            StatName statName = statValue.stat;
            GetStat(statName).statValue = statValue.value;
        }
        if (agent != null) {
            Debug.LogError("Tried to redefine an AgentUI");
        } else {
            agent = newAgent;
        }
    }
    public StatUI GetStat(StatName statName)
    {
        foreach (var stat in stats)
        {
            if (stat.statName == statName) { return stat; }
        }
        return null;
    }
    public void Update()
    {
        foreach (var statUI in stats)
        {
            var statName = statUI.statName;
            statUI.statValue = (agent != null) ? agent.GetStat(statName) : -1;
        }
    }
}