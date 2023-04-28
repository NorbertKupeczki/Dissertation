using static Utility.Utility;
using GeneralData;
using UnityEngine;

public class Personality
{
    private bool _traitsSet = false;
    
    public Personality(bool randomise)
    {
        if (randomise) RandomGeneratePersonality();
        _traitsSet = randomise;
    }

    private int _openness; // inventive/curious vs. consistent/cautious
    private int _conscientiousness; // efficient/organized vs. extravagant/careless
    private int _extraversion; // outgoing/energetic vs. solitary/reserved
    private int _agreeableness; // friendly/compassionate vs. critical/rational
    private int _neuroticism; // sensitive/nervous vs. resilient/confident

    #region >> Getters
    public int Openness => _openness;

    public int Conscientiousness => _conscientiousness;

    public int Extraversion => _extraversion;

    public int Agreementableness => _agreeableness;

    public int Neuroticism => _neuroticism;
    #endregion

    #region >> Setters
    public void SetOpenness(int value) => _openness = value;

    public void SetConscientiousness(int value) => _conscientiousness = value;

    public void SetExraversion(int value) => _extraversion = value;

    public void SetAgreeableness(int value) => _agreeableness = value;

    public void SetNeuroticism(int value) => _neuroticism = value;
    #endregion

    /// <summary>
    /// Randomly generates the agent's personality traits.
    /// </summary>
    public void RandomGeneratePersonality()
    {
        SetOpenness(GetRandomTrait());
        SetConscientiousness(GetRandomTrait());
        SetExraversion(GetRandomTrait());
        SetAgreeableness(GetRandomTrait());
        SetNeuroticism(GetRandomTrait());
    }

    /// <summary>
    /// Returns the impact of an event on the agent based on the provided interaction data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns>Int</returns>
    public int ReceiveInteraction(InteractionDataSO data)
    {
        float relationChange = EvaluateAllTraitsVsEvents(data) * data.Impact;
        return Mathf.RoundToInt(relationChange);
    }

    /// <summary>
    /// Sets the personality traits of the agent, returns true if the setting of data was successful, false
    /// if the traits have already been set.
    /// </summary>
    /// <param name="openness"></param>
    /// <param name="conscientiousness"></param>
    /// <param name="extraversion"></param>
    /// <param name="agreeableness"></param>
    /// <param name="neuroticism"></param>
    /// <returns>Bool</returns>
    public bool SetPersonalityTraits(int openness, int conscientiousness, int extraversion, int agreeableness, int neuroticism)
    {
        if (_traitsSet) return false;
        
        SetOpenness(openness);
        SetConscientiousness(conscientiousness);
        SetExraversion(extraversion);
        SetAgreeableness(agreeableness);
        SetNeuroticism(neuroticism);

        return _traitsSet = true;
    }

    /// <summary>
    /// Evaluates all the personality traits compared to the event data, and returns a multiplier that
    /// is to be used to determine the actual effect of the event.
    /// </summary>
    /// <param name="data"></param>
    /// <returns>Float</returns>
    private float EvaluateAllTraitsVsEvents(InteractionDataSO data)
    {
        float result = EvaluateEventVersusTrait(data.Openness, _openness);
        result += EvaluateEventVersusTrait(data.Conscientiousness, _conscientiousness);
        result += EvaluateEventVersusTrait(data.Extraversion, _extraversion);
        result += EvaluateEventVersusTrait(data.Agreeableness, _agreeableness);
        result += EvaluateEventVersusTrait(data.Neuroticism, _neuroticism);

        return result * 0.2f;
    }

    /// <summary>
    /// Evaluates an individual trait vs. the event data.
    /// </summary>
    /// <param name="eventValue"></param>
    /// <param name="traitValue"></param>
    /// <returns>Float</returns>
    private float EvaluateEventVersusTrait(int eventValue, int traitValue)
    {
        float traitFactor = (traitValue - Data.EVALUATION_FACTOR) * 0.01f;
        float eventFactor = (eventValue - Data.EVALUATION_FACTOR) * 0.01f;

        return 1 + traitFactor + eventFactor;
    }
}
