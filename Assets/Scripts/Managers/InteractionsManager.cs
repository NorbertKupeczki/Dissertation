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

    public void RegisterInteractionEvent(Vector3 position, InteractionDataSO eventData, List<Agent> affectedAgents)
    {
        InteractionEvent newEvent = new(position, eventData, affectedAgents, Time.realtimeSinceStartup, _interactionHistory.Count);
        _interactionHistory.Add(newEvent, newEvent.EventId);
        RegisterEventWithAffectedAgents(newEvent); 
    }

    public int FindEventInHistory(InteractionEvent eventData)
    {
        if (_interactionHistory.TryGetValue(eventData, out var history))
        {
            return history;
        }
        return -1;
    }

    public InteractionEvent GetEventFromHistoryByIndex(int index)
    {
        return _interactionHistory.ElementAt(index).Key;
    }

    private void RegisterEventWithAffectedAgents(InteractionEvent @event)
    {
        foreach (Agent agent in @event.AffectedAgents)
        {
            agent.ProcessInteractionEvent(@event);
        }
    }
}
