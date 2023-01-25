using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header ("Body parts")]
    [SerializeField] private GameObject _hair;
    [SerializeField] private GameObject _skin;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _maleHairObj;
    [SerializeField] private GameObject _femaleHairObj;

    [Header("Attributes")]
    [SerializeField] private Gender _gender = Gender.UNDEFINED;
    [SerializeField] private List<GameObject> _contacts = new List<GameObject>();

    private enum Gender
    {
        UNDEFINED = 0,
        MALE = 1,
        FEMALE = 2
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
        return (Gender)Random.Range(1, 3);
    }

    /// <summary>
    /// Searches the list of contacts for a GameObject.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns>Returns true if the contact is found</returns>
    public bool CheckContact(GameObject contact)
    {
        return _contacts.Contains(contact);
    }

    /// <summary>
    /// Adds a new conact to the lists of contacts if it doesn't already
    /// contains the contact.
    /// </summary>
    /// <param name="newContact"></param>
    public void RegisterContact(GameObject newContact)
    {
        if (!CheckContact(newContact))
        {
            _contacts.Add(newContact);
        }
    }
}
