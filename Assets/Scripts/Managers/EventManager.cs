using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action<Agent> AgentSelected;
    public static event Action Deselect;
    public static event Action<Vector3> Move;
    public static event Action PositiveAreaEffect;
    public static event Action NegativeAreaEffect;

    public static event Action<Vector3, InteractionDataSO, List<Agent>> RegisterInteractionEvent;

    public static event Action<Vector3> MoveCamera;
    public static event Action<float> RotateCamera;

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

    public static void OnPositiveAreaEffect()
    {
        PositiveAreaEffect?.Invoke();
    }
    public static void OnNegativeAreaEffect()
    {
        NegativeAreaEffect?.Invoke();
    }

    public static void OnInteractionEvent(Vector3 position, InteractionDataSO eventData, List<Agent> affectedAgents)
    {
        RegisterInteractionEvent?.Invoke(position, eventData, affectedAgents);
    }

    public static void OnCameraControlMoveInput(Vector3 vector)
    {
        MoveCamera?.Invoke(vector);
    }

    public static void OnCameraControlRotateInput(float rotation)
    {
        RotateCamera?.Invoke(rotation);
    }
}
