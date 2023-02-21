using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<Agent> _contacts = new List<Agent>();

    private Dictionary<Agent, int> _contactRelations = new Dictionary<Agent, int>();

    public enum Gender
    {
        UNDEFINED = 0,
        MALE = 1,
        FEMALE = 2
    }

    /// <summary>
    /// Gets the agent's gender
    /// </summary>
    /// <returns>Returns the agent's gender</returns>
    public Gender GetGender()
    {
        return _gender;
    }

    private void Awake()
    {
        _gender = GetRandomSex();
        if (_gender == Gender.MALE)
        {
            _hair = Instantiate(_maleHairObj,_skin.transform.position, Quaternion.Euler(-90.0f,0.0f,0.0f), gameObject.transform);
        }
        else
        {
            _hair = Instantiate(_femaleHairObj, _skin.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f), gameObject.transform);
        }

        _hair.name = "Hair";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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

    private Gender GetRandomSex()
    {
        return (Gender)UnityEngine.Random.Range(1, Enum.GetNames(typeof(Gender)).Length);
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
    public bool RegisterContact(Agent newContact)
    {
        if (!CheckContact(newContact))
        {
            _contacts.Add(newContact);
            _contactRelations.Add(newContact, 50);
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
    /// Searches for the agent with the highest relationship score
    /// </summary>
    /// <returns>Returns the agent with the highest relationshio score, returns null if the dictionary is empty</returns>
    private Agent FindHighestRelationContact()
    {
        if (_contactRelations.Count < 1)
        {
            return null;
        }

        int relationScore = 101;
        Agent bestContact = null;

        foreach (KeyValuePair<Agent, int> contact in _contactRelations)
        {
            if (contact.Value < relationScore)
            {
                bestContact = contact.Key;
            }
        }

        return bestContact;
    }
}
