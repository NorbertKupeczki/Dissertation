using System;
using UnityEngine;

public static class EventManager
{
    public static event Action<Agent> AgentSelected;
    public static event Action Deselect;
    public static event Action<Vector3> Move;
    public static event Action AreaEffect1;

    public static void OnAgentSelected(Agent agent)
    {
        AgentSelected?.Invoke(agent);
    }

    public static void OnDeselect()
    {
        Deselect?.Invoke();
    }

    public static void OnMiddleMouseClick(Vector3 position)
    {
        Move?.Invoke(position);
    }

    public static void OnAreaEffect1()
    {
        AreaEffect1?.Invoke();
    }
}
