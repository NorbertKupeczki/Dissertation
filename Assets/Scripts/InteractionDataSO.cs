using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractionData", menuName = "Interaction/NewDataObject")]
public class InteractionDataSO : ScriptableObject
{
    public string Name;
    [Space]
    public int Impact;
    [Space]
    public int Openness;
    public int Conscientiousness;
    public int Extraversion;
    public int Agreeableness;
    public int Neuroticism;
}
