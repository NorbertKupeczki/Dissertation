using System;
using System.Collections.Generic;
using GeneralData;
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
    [SerializeField] private RelationLine _relationLinePrefab;

    [Header("Particle Systems")]
    [SerializeField] private ParticleController _positiveEffect;
    [SerializeField] private ParticleController _negativeEffect;

    private List<RelationLine> _relationLines = new();
    private bool _relationLinesOn = false;

    private Personality _personality = null;
    private Dictionary<Agent, int> _contactRelations = new();

    private InteractionsMemory _memory = new();
    private int _internalConnections = 0;
    private Gossip _gossip;

    private int _clusterID;

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
        _gossip = GetComponent<Gossip>();
        _gossip.SetLinkToMemory(_memory);
    }

    private void Start()
    {
        _relationToPlayer = Data.STARTING_PLAYER_RELATION;

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

#region >> Event registration functions
    private void SubscribeEvents()
    {
        EventManager.AgentSelected += EnableRelationLines;
        EventManager.Deselect += DisableRelationLines;
    }

    private void UnsubscribeEvents()
    {
        EventManager.AgentSelected -= EnableRelationLines;
        EventManager.Deselect -= DisableRelationLines;
    }
    #endregion
    
    /// <summary>
    /// Sets the cluster ID
    /// </summary>
    /// <param name="value"></param>
    private void SetClusterID(int value)
    {
        _clusterID = value;
    }

    /// <summary>
    /// Returns the cluster ID
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Int</returns>
    public bool CheckClusterID(int value)
    {
        return _clusterID == value;
    }

    /// <summary>
    /// Initialises the agent.
    /// </summary>
    /// <param name="hairColour"></param>
    /// <param name="skinColour"></param>
    /// <param name="bodyColour"></param>
    /// <param name="clusterID"></param>
    public void InitAgent(Color hairColour, Color skinColour, Color bodyColour, int clusterID)
    {
        _hair.GetComponent<Renderer>().material.color = hairColour;
        _skin.GetComponent<Renderer>().material.color = skinColour;
        _body.GetComponent<Renderer>().material.color = bodyColour;
        SetClusterID(clusterID);
    }

    /// <summary>
    /// Searches the list of contacts for an Agent.
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
        if (CheckContact(newContact)) return false;
        _contacts.Add(newContact);
        _contactRelations.Add(newContact, relationValue);

        RelationLine newLine = Instantiate(_relationLinePrefab, gameObject.transform);
        newLine.SetLineEnd(newContact.transform);
        newLine.SetLineStart(gameObject.transform);
        _relationLines.Add(newLine);
            
        SortContacts();
        return true;
    }

    /// <summary>
    /// Removes a contact from the agent's list of contacts.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns>Returns true if the removal of contact was successful</returns>
    public bool RemoveContact(Agent contact)
    {
        if (!_contacts.Contains(contact)) return false;
        _contacts.Remove(contact);
        _contactRelations.Remove(contact);

        //Todo: Add removal of relationship line!

        SortContacts();
        return true;
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
        
        _contactRelations[contact] = Mathf.Clamp(_contactRelations[contact] + value,
                                                 Data.RELATIONSHIP_SCORE_MIN,
                                                 Data.RELATIONSHIP_SCORE_MAX);

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
    public List<Agent> GetContactList()
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

    /// <summary>
    /// Sets the number of internal connections
    /// </summary>
    public void SetNumberOfInternalConnections()
    {
        _internalConnections = _contacts.Count;
    }

    /// <summary>
    /// Turns on the relationship lines of the agent.
    /// </summary>
    /// <param name="agent"></param>
    private void EnableRelationLines(Agent agent)
    {
        if (agent != this) return;
        ToggleRelationLines(true);
        _relationLinesOn = true;
    }

    /// <summary>
    /// Turns off the relationship lines of the agent
    /// </summary>
    private void DisableRelationLines()
    {
        if (!_relationLinesOn) return;
        ToggleRelationLines(false);
        _relationLinesOn = false;
    }

    /// <summary>
    /// Toggles the relationship lines of the agent ON/Off
    /// </summary>
    /// <param name="value"></param>
    private void ToggleRelationLines(bool value)
    {
        foreach (RelationLine line in _relationLines)
        {
            line.ToggleLine(value);
        }
    }

    /// <summary>
    /// Takes the InteractionEvent data and processes its information to change the agents
    /// relation with the player.
    /// </summary>
    /// <param name="eventData"></param>
    public void ProcessInteractionEvent(InteractionEvent eventData)
    {
        // 1. Check event in memory, if it is already registered, do nothing, return
        if(_memory.CheckMemoryForEvent(eventData)) return;
        // 2. If the event is not registered, register it, then check event data against personality to determine the impact
        _memory.RegisterMemory(eventData);
        int impactOnRelation = _personality.ReceiveInteraction(eventData.Event);
        // 3. Based on data returned, change the relationship score
        // 4. Based on relationship score change, play particle effect
        RelationToPlayerChange(impactOnRelation);
    }

    /// <summary>
    /// Display the relationship changes initiating the matching particle effect, also
    /// changes the relationship value.
    /// </summary>
    /// <param name="value"></param>
    public void RelationToPlayerChange(int value)
    {
        if (value > 0) PlayPositiveParticleEffect(EvaluateImpact(value));
        else if (value < 0) PlayNegativeParticleEffect(EvaluateImpact(value));

        ChangeRelataionScoreToPlayer(value);
    }

    /// <summary>
    /// Changes the agents relation with the player by adding the parameter, clamped by the minimum and maximum value possible.
    /// </summary>
    /// <param name="value"></param>
    private void ChangeRelataionScoreToPlayer(int value)
    {
        _relationToPlayer = Mathf.Clamp(_relationToPlayer + value, Data.RELATIONSHIP_SCORE_MIN, Data.RELATIONSHIP_SCORE_MAX);
    }

    /// <summary>
    /// Evaluates the impact category of a relationship change.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>EffectImpact</returns>
    private EffectImpact EvaluateImpact(int value)
    {
        int absValue = Mathf.Abs(value);
        switch (absValue)
        {
            case <= 5:
                return EffectImpact.LOW_IMPACT;
            case <= 9:
                return EffectImpact.MEDIUM_IMPACT;
            default:
                return EffectImpact.HIGH_IMPACT;
        }
    }

    /// <summary>
    /// Plays the particle effect showing positive change in relations
    /// </summary>
    /// <param name="impact"></param>
    private void PlayPositiveParticleEffect(EffectImpact impact)
    {
        switch (impact) 
        {
            case EffectImpact.LOW_IMPACT:
                _positiveEffect.SpawnParticlesLowAmount();
                break;
            case EffectImpact.MEDIUM_IMPACT:
                _positiveEffect.SpawnParticlesMediumAmount();
                break;
            case EffectImpact.HIGH_IMPACT:
                _positiveEffect.SpawnParticlesHighAmount();
                break;
        }
    }

    /// <summary>
    /// Plays the particle effect showing negative change in relations
    /// </summary>
    /// <param name="impact"></param>
    private void PlayNegativeParticleEffect(EffectImpact impact)
    {
        switch (impact)
        {
            case EffectImpact.LOW_IMPACT:
                _negativeEffect.SpawnParticlesLowAmount();
                break;
            case EffectImpact.MEDIUM_IMPACT:
                _negativeEffect.SpawnParticlesMediumAmount();
                break;
            case EffectImpact.HIGH_IMPACT:
                _negativeEffect.SpawnParticlesHighAmount();
                break;
        }
    }

    /// <summary>
    /// Initialises the gossip module
    /// </summary>
    public void InitGossipModule()
    {
        for(int i = 0; i < _contacts.Count ; ++i)
        {
            if (_contacts[i].CheckClusterID(_clusterID))
            {
                _gossip.AddContactToList(_contacts[i], true);
                continue;
            }
            _gossip.AddContactToList(_contacts[i], false);
        }
    }
}
