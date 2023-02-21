using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility.Utility;

public class Cluster : MonoBehaviour
{
    [SerializeField] private List<Agent> _members = new List<Agent>();
    [SerializeField] private List<ContactPairs> _contacts = new List<ContactPairs>();
    [SerializeField] public Color _clusterColour { get; private set; }
    
    private void Awake()
    {
        _clusterColour = GetRandomColour();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ContactPairs pair in _contacts)
        {
            pair.DrawConnectionLine();
        }
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

    private struct ContactPairs
    {
        public Agent firstContact;
        public Agent secondContact;

        public void DrawConnectionLine()
        {
            Debug.DrawLine(firstContact.transform.position, secondContact.transform.position,Color.green);
        }
    }

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
    }
}
