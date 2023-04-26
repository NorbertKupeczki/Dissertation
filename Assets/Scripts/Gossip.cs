using System.Collections;
using System.Collections.Generic;
using GeneralData;
using UnityEngine;

public class Gossip : MonoBehaviour
{
    private List<Agent> _clusterContacts = new();
    private List<Agent> _externalContacts = new();
    private int _totalContacts = 0;
    private InteractionsMemory _memory;
    //private Agent _thisAgent;

    private float _clusterGossipCooldown;
    private float _externalGossipCooldown;

    private void Awake()
    {
        //_thisAgent = GetComponent<Agent>();

        ResetClusterCooldown();
        ResetExternalCooldown();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGossipCooldowns();
    }

    public void SetLinkToMemory(InteractionsMemory memory)
    {
        _memory = memory;
    }

    public void AddContactToList(Agent contact, bool internalContact)
    {
        if (internalContact)
        {
            _clusterContacts.Add(contact);
        }
        else
        {
            _externalContacts.Add(contact);
        }

        _totalContacts += 1;
    }

    /// <summary>
    /// Finds an event in the current agent's memory that hasn't been shared with a cluster,
    /// if there is one, the information is shared with every member of the cluster.
    /// </summary>
    private void GossipWithinCluster()
    {
        if(!_memory.FindUntoldGossip(_clusterContacts[0], out InteractionEvent eventData)) return;

        foreach (Agent contact in _clusterContacts)
        {
            contact.ProcessInteractionEvent(eventData);
            _memory.AddAgentToGossipList(eventData, contact);
        }
    }

    private void GossipOutsideCluster()
    {
        Agent contact = _externalContacts[Random.Range(0, _externalContacts.Count)];

        if (!_memory.FindUntoldGossip(contact, out InteractionEvent eventData)) return;
        contact.ProcessInteractionEvent(eventData);
        _memory.AddAgentToGossipList(eventData, contact);
    }

    private void ResetClusterCooldown()
    {
        _clusterGossipCooldown = Random.Range(GossipData.GOSSIP_DELAY_MIN, GossipData.GOSSIP_DELAY_MAX) * GossipData.GOSSIP_CLUSTER_TIME_MULTIPLIER;
    }

    private void ResetExternalCooldown()
    {
        _externalGossipCooldown = Random.Range(GossipData.GOSSIP_DELAY_MIN, GossipData.GOSSIP_DELAY_MAX);
    }

    private void UpdateGossipCooldowns()
    {
        float deltaTime = Time.deltaTime;

        _clusterGossipCooldown -= deltaTime;
        _externalGossipCooldown -= deltaTime;

        if (_clusterGossipCooldown <= 0.0f)
        {
            GossipWithinCluster();
            ResetClusterCooldown();
        }

        if (_externalGossipCooldown <= 0.0f)
        {
            GossipOutsideCluster();
            ResetExternalCooldown();
        }
    }
}
