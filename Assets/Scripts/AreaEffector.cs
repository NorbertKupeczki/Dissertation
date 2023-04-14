using System.Collections.Generic;
using UnityEngine;

public class AreaEffector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Move += Move;
        EventManager.AreaEffect1 += ApplyEffectOnAgents;
    }

    public void Move(Vector3 position)
    {
        transform.position = new Vector3(position.x, 0.0f, position.z);
    }

    private List<Agent> GetAgentsInArea()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 4.0f, LayerMask.GetMask("Agent"));
        if (hits.Length < 1) return null;
        List<Agent> agentsHit = new(hits.Length);
        foreach (Collider hit in hits)
        {
            agentsHit.Add(hit.GetComponent<Agent>());
        }

        return agentsHit;
    }

    private void ApplyEffectOnAgents()
    {
        List<Agent> agents = GetAgentsInArea();
        if(agents == null) return;
        foreach (Agent agent in agents)
        {
            Debug.Log(agent.gameObject.name);
        }
    }
}
