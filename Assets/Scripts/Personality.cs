using static Utility.Utility;

public class Personality
{
    public Personality()
    {
        RandomGeneratePersonality();
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

    public void RandomGeneratePersonality()
    {
        _openness = GetRandomTrait();
        _conscientiousness = GetRandomTrait();
        _extraversion = GetRandomTrait();
        _agreeableness = GetRandomTrait();
        _neuroticism = GetRandomTrait();
    }

    public int ReceiveInteraction(InteractionDataSO data)
    {
        // Evaluates data, returns the value of the change based on the
        // agent's personality and the event's properties
        return 0;
    }
}
