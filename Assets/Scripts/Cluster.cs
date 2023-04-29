using System.Collections.Generic;
using UnityEngine;
using static Utility.Utility;

public class Cluster : MonoBehaviour
{
    [SerializeField] private List<Agent> _members = new();
    [SerializeField] private List<ContactPairs> _contacts = new();
    [SerializeField] public Color _clusterColour { get; private set; }
    
    private void Awake()
    {
        _clusterColour = GetRandomColour();
    }
    
    private struct ContactPairs
    {
        public Agent firstContact;
        public Agent secondContact;
    }

    /// <summary>
    /// Returns the list of members of the cluster
    /// </summary>
    /// <returns>List of Agent</returns>
    public List<Agent> GetMembersList()
    {
        return _members;
    }

    /// <summary>
    /// Returns a random member of the cluster
    /// </summary>
    /// <returns>Agent</returns>
    public Agent GetRandomMember()
    {
        return _members[RollADice(_members.Count -1)];
    }

    /// <summary>
    /// Adds a new member to the list of cluster members
    /// if the list doesn't already contain the member.
    /// </summary>
    /// <param name="newMember"></param>
    public void RegisterMember(Agent newMember)
    {
        if(!_members.Contains(newMember))
        {
            _members.Add(newMember);
        }
    }

    /// <summary>
    /// Generates the connections within the cluster
    /// </summary>
    public void GenerateConnections()
    {
        for (int i = 0; i < _members.Count; i++)
        {
            int a;
            int b;

            do
            {
                a = Random.Range(0, _members.Count);
            }
            while (a == i);

            AddPairsToContacts(i, a);

            do
            {
                b = Random.Range(0, _members.Count);
            }
            while (b == i && b == a);

            AddPairsToContacts(i, b);
        }

    }

    /// <summary>
    /// Pairs the members of the cluster using their IDs.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void AddPairsToContacts(int x, int y)
    {
        foreach (ContactPairs pair in _contacts)
        {
            if (pair.firstContact == _members[x] && pair.secondContact == _members[y] ||
                pair.firstContact == _members[y] && pair.secondContact == _members[x])
            {
                return;
            }
        }

        _contacts.Add(new ContactPairs
        {
            firstContact = _members[x],
            secondContact = _members[y]
        });

        MutuallyRegisterContacts(_members[x], _members[y]);
    }

    /// <summary>
    /// Generates contacts for each agents from outside their cluster.
    /// </summary>
    /// <param name="clusters"></param>
    public void GenerateInterClusterConnections(List<Cluster> clusters)
    {
        int connections = 0;
        Cluster targetCluster;
        Agent targetAgent;

        foreach (Agent member in _members)
        {
            member.SetNumberOfInternalConnections();
            connections = RollARealDice(clusters.Count);

            for (int i = 0; i < connections; ++i)
            {
                // 1. Pick a cluster different to this
                targetCluster = PickRandomCluster(clusters);
                // 2. Pick a random member
                targetAgent = targetCluster.GetRandomMember();
                // 3. Check if this and the picked member are contacts already, if yes, skip
                if (member.CheckContact(targetAgent)) continue;
                // 4. If not, make them contacts
                MutuallyRegisterContacts(member, targetAgent);
            }
        }
    }

    /// <summary>
    /// Picks a random cluster from a List provided that is different from the caller
    /// </summary>
    /// <param name="clusters"></param>
    /// <returns>Cluster</returns>
    private Cluster PickRandomCluster(List<Cluster> clusters)
    {
        Cluster result;

        do
        {
            int randomIndex = RollADice(clusters.Count - 1);
            result = clusters[randomIndex];
        } while (result == this);

        return result;
    }

    /// <summary>
    /// Mutually registers two agents as each others contacts.
    /// </summary>
    /// <param name="AgentOne"></param>
    /// <param name="AgentTwo"></param>
    private void MutuallyRegisterContacts(Agent AgentOne, Agent AgentTwo)
    {
        int relationScore = RollARealDice(100);

        AgentOne.RegisterContact(AgentTwo, relationScore);
        AgentTwo.RegisterContact(AgentOne, relationScore);
    }
}
