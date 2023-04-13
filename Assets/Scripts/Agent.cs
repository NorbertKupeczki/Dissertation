using System;
using System.Collections.Generic;
using UnityEngine;
using static Utility.Utility;

public class Agent : MonoBehaviour
{
    [Header("Body parts")]
    [SerializeField] private GameObject _hair;
    [SerializeField] private GameObject _skin;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _maleHairObj;
    [SerializeField] private GameObject _femaleHairObj;

    [Header("Attributes")]
    [SerializeField] private Gender _gender = Gender.UNDEFINED;
    [SerializeField] private List<Agent> _contacts = new ();

    [Header("Relation to player")]
    [SerializeField] private int _relationToPlayer;

    private Personality _personality = null;
    private Dictionary<Agent, int> _contactRelations = new();

    /// <summary>
    /// Gets the agent's gender
    /// </summary>
    /// <returns>Returns the agent's gender</returns>
    public Gender GetGender => _gender;

    /// <summary>
    /// Gets the agent's relationship with the player
    /// </summary>
    public int RelationToPlayer => _relationToPlayer;
    
    /// <summary>
    /// Accesses the class storing the agent's personality data
    /// </summary>
    public Personality Personality => _personality;

    private void Awake()
    {
        _gender = GetRandomSex();
        if (_gender == Gender.MALE)
        {
            _hair = Instantiate(_maleHairObj, _skin.transform.position, Quaternion.Euler(s_blenderRotation), gameObject.transform);
        }
        else
        {
            _hair = Instantiate(_femaleHairObj, _skin.transform.position, Quaternion.Euler(s_blenderRotation), gameObject.transform);
        }

        _hair.name = "Hair";

        _personality = new Personality();
    }

    private void Start()
    {
        _relationToPlayer = GetRandomTrait();
    }

    /// <summary>
    /// Initialises the colours of the agent.
    /// </summary>
    /// <param name="hairColour"></param>
    /// <param name="skinColour"></param>
    /// <param name="bodyColour"></param>
    public void InitAgent(Color hairColour, Color skinColour, Color bodyColour)
    {
        _hair.GetComponent<Renderer>().material.color = hairColour;
        _skin.GetComponent<Renderer>().material.color = skinColour;
        _body.GetComponent<Renderer>().material.color = bodyColour;
    }

    /// <summary>
    /// Searches the list of contacts for a GameObject.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns>Returns true if the contact is found</returns>
    public bool CheckContact(Agent contact)
    {
        return _contacts.Contains(contact);
    }

    /// <summary>
    /// Adds a new conact to the lists of contacts if it doesn't already
    /// contains the contact.
    /// </summary>
    /// <param name="newContact"></param>
    public bool RegisterContact(Agent newContact, int relationValue)
    {
        if (!CheckContact(newContact))
        {
            _contacts.Add(newContact);
            _contactRelations.Add(newContact, relationValue);

            SortContacts();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes a contact from the agent's list of contacts.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns>Returns true if the removal of contact was successful</returns>
    public bool RemoveContact(Agent contact)
    {
        if (_contacts.Contains(contact))
        {
            _contacts.Remove(contact);
            _contactRelations.Remove(contact);

            SortContacts();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds the value to the contact's relationship value
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="value"></param>
    public void ChangeRelationWithContact(Agent contact, int value)
    {
        if (!_contacts.Contains(contact))
        {
            return;
        }

        if (_contactRelations[contact] + value < 0)
        {
            _contactRelations[contact] = 0;
        }
        else if (_contactRelations[contact] + value > 100)
        {
            _contactRelations[contact] = 100;
        }
        else
        {
            _contactRelations[contact] += value;
        }

        SortContacts();
    }

    /// <summary>
    /// Checks the relationship score with another agent
    /// </summary>
    /// <param name="contact"></param>
    /// <returns>Returns a value between 0 and 100, returns -1 if the contact is unknown</returns>
    public int CheckRelationWithContact(Agent contact)
    {
        if (_contacts.Contains(contact))
        {
            return _contactRelations[contact];
        }
        return -1;
    }

    /// <summary>
    /// Sorts the contacts in a descending order based on the relationship scores
    /// </summary>
    private void SortContacts()
    {
        if (_contactRelations.Count < 2)
        {
            return;
        }

        _contacts.Sort(delegate (Agent a, Agent b)
        {
            return (_contactRelations[b].CompareTo(_contactRelations[a]));
        });
    }

    /// <summary>
    /// Returns the agent's list of contacts
    /// </summary>
    /// <returns></returns>
    public List<Agent> GetContacList()
    {
        return _contacts;
    }

    /// <summary>
    /// Returns the contact with the highest relationship value
    /// </summary>
    /// <returns>Returns null if the agent has no contacts</returns>
    public Agent GetBestContact()
    {
        if (_contacts.Count > 1)
        {
            return _contacts[0];
        }
        return null;
    }

    /// <summary>
    /// Returns the contact with the lowest relationship value
    /// </summary>
    /// <returns>Returns null if the agent has no contacts</returns>
    public Agent GetWorstContact()
    {
        if (_contacts.Count > 1)
        {
            return _contacts[^1];
        }
        return null;
    }
}
