using System.Collections.Generic;
using GeneralData;
using UnityEngine;

public class AreaEffector : MonoBehaviour
{
    void Start()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

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

    public void Move(Vector3 position)
    {
        transform.position = new Vector3(position.x, 0.0f, position.z);
    }

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

    private void ApplyPositiveAreaEffect()
    {
        List<Agent> agents = GetAgentsInArea();
        if(agents == null) return;
        foreach (Agent agent in agents)
        {
            agent.RelationChangeTest(1);
        }
    }

    private void ApplyNegativeAreaEffect()
    {
        List<Agent> agents = GetAgentsInArea();
        if (agents == null) return;
        foreach (Agent agent in agents)
        {
            agent.RelationChangeTest(-1);
        }
    }
}
