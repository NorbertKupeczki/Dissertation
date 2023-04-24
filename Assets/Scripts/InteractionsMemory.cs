using GeneralData;
using System.Collections.Generic;

public class InteractionsMemory
{
    private Dictionary<InteractionEvent, int> _agentMemory = new();

    public void RegisterMemory(InteractionEvent @event)
    {
        _agentMemory.Add(@event, _agentMemory.Count);
    }

    public bool CheckMemoryForEvent(InteractionEvent @event)
    {
        return _agentMemory.ContainsKey(@event);
    }
}
