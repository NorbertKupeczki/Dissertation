using System;

public static class EventManager
{
    public static event Action<Agent> AgentSelected;
    public static event Action Deselect;

    public static void OnAgentSelected(Agent agent)
    {
        AgentSelected?.Invoke(agent);
    }

    public static void OnDeselect()
    {
        Deselect?.Invoke();
    }
}
