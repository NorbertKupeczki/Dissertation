using System.Collections.Generic;
using GeneralData;
using UnityEngine;

public class AreaEffector : MonoBehaviour
{
    [SerializeField] public InteractionDataSO _positiveInteractionData;
    [SerializeField] public InteractionDataSO _negativeInteractionData;

    void Start()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

#region >> Event registration functions
    private void SubscribeEvents()
    {
        EventManager.Move += Move;
        EventManager.PositiveAreaEffect += ApplyPositiveAreaEffect;
        EventManager.NegativeAreaEffect += ApplyNegativeAreaEffect;
    }

    private void UnsubscribeEvents()
    {
        EventManager.Move -= Move;
        EventManager.PositiveAreaEffect -= ApplyPositiveAreaEffect;
        EventManager.NegativeAreaEffect -= ApplyNegativeAreaEffect;
    }
#endregion

    /// <summary>
    /// Moves the zone marker to the location provided as a parameter
    /// </summary>
    /// <param name="position"></param>
    public void Move(Vector3 position)
    {
        transform.position = new Vector3(position.x, 0.0f, position.z);
    }

    /// <summary>
    /// Returns all agents in the effected area
    /// </summary>
    /// <returns></returns>
    private List<Agent> GetAgentsInArea()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Data.AOE_DETECTION_RADIUS, LayerMask.GetMask("Agent"));
        if (hits.Length < 1) return null;
        List<Agent> agentsHit = new(hits.Length);
        foreach (Collider hit in hits)
        {
            agentsHit.Add(hit.GetComponent<Agent>());
        }

        return agentsHit;
    }

    /// <summary>
    /// Applies the positive effect on the agents within range
    /// </summary>
    private void ApplyPositiveAreaEffect()
    {
        List<Agent> agents = GetAgentsInArea();
        if(agents == null) return;

        EventManager.OnInteractionEvent(transform.position, _positiveInteractionData, agents );
    }

    /// <summary>
    /// Applies the negative effect on the agents within range
    /// </summary>
    private void ApplyNegativeAreaEffect()
    {
        List<Agent> agents = GetAgentsInArea();
        if (agents == null) return;

        EventManager.OnInteractionEvent(transform.position, _negativeInteractionData, agents);
    }
}
