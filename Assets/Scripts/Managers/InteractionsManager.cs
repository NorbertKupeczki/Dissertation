using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GeneralData;

public class InteractionsManager : MonoBehaviour
{
    private Dictionary<InteractionEvent, int> _interactionHistory = new();

    private void Start()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

#region >> Event registration functions
    private void RegisterEvents()
    {
        EventManager.RegisterInteractionEvent += RegisterInteractionEvent;
    }

    private void UnregisterEvents()
    {
        EventManager.RegisterInteractionEvent -= RegisterInteractionEvent;
    }
#endregion

    /// <summary>
    /// Registers the event, then delegates the processing of it to the agents affected.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="eventData"></param>
    /// <param name="affectedAgents"></param>
    public void RegisterInteractionEvent(Vector3 position, InteractionDataSO eventData, List<Agent> affectedAgents)
    {
        InteractionEvent newEvent = new(position, eventData, affectedAgents, Time.realtimeSinceStartup, _interactionHistory.Count);
        _interactionHistory.Add(newEvent, newEvent.EventId);
        RegisterEventWithAffectedAgents(newEvent); 
    }

    /// <summary>
    /// Searches for an event and returns its registration ID, or -1 if the event doesn't exist.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public int FindEventInHistory(InteractionEvent eventData)
    {
        if (_interactionHistory.TryGetValue(eventData, out var history))
        {
            return history;
        }
        return -1;
    }

    /// <summary>
    /// Returns an event based on the index provided.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>InteractionEvent</returns>
    public InteractionEvent GetEventFromHistoryByIndex(int index)
    {
        return _interactionHistory.ElementAt(index).Key;
    }

    /// <summary>
    /// Passes the event data to the target agents.
    /// </summary>
    /// <param name="eventData"></param>
    private void RegisterEventWithAffectedAgents(InteractionEvent eventData)
    {
        foreach (Agent agent in eventData.AffectedAgents)
        {
            agent.ProcessInteractionEvent(eventData);
        }
    }
}
