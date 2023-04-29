using GeneralData;
using System.Collections.Generic;

public class InteractionsMemory
{
    private Dictionary<InteractionEvent, List<Agent>> _agentMemory = new();

    public void RegisterMemory(InteractionEvent eventData)
    {
        _agentMemory.Add(eventData, new());
    }

    /// <summary>
    /// Checks if an agent has an event in its memory.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns>bool</returns>
    public bool CheckMemoryForEvent(InteractionEvent eventData)
    {
        return _agentMemory.ContainsKey(eventData);
    }

    /// <summary>
    /// Returns the gossip list of an event.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns>List of Agent</returns>
    public List<Agent> GetGossipList(InteractionEvent eventData)
    {
        _agentMemory.TryGetValue(eventData, out List<Agent> gossipList);
        return gossipList;
    }

    /// <summary>
    /// Checks if an agent is on an event's gossip list, returns true if the agent is on this list.
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="agent"></param>
    /// <returns>Bool</returns>
    public bool CheckAgentOnGossipList(InteractionEvent eventData, Agent agent)
    {
        return _agentMemory[eventData].Contains(agent);
    }

    /// <summary>
    /// Adds agent to an events gossip list, returns true if the insertion was successful,
    /// false if the agent was on the list already.
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="agent"></param>
    /// <returns>Bool</returns>
    public bool AddAgentToGossipList(InteractionEvent eventData, Agent agent)
    {
        if (_agentMemory[eventData].Contains(agent)) return false;
        _agentMemory[eventData].Add(agent);
        return true;
    }
    
    /// <summary>
    /// Returns bool if there is an InteractionEvent that doesn't have the Agent parameter
    /// registered with it. The first event found is then returned as an out parameter.
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="eventData"></param>
    /// <returns>Bool</returns>
    public bool FindUntoldGossip(Agent agent, out InteractionEvent eventData)
    {
        foreach (KeyValuePair<InteractionEvent, List<Agent>> element in _agentMemory)
        {
            if (element.Value.Contains(agent)) continue;
            eventData = element.Key;
            return true;
        }

        eventData = new();
        return false;
    }
}
