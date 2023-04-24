using System.Collections.Generic;
using UnityEngine;

public class InteractionButtonsSingle : MonoBehaviour
{
    [SerializeField] private InteractionDataSO _positiveData1;
    [SerializeField] private InteractionDataSO _positiveData2;
    [SerializeField] private InteractionDataSO _negativeData1;
    [SerializeField] private InteractionDataSO _negativeData2;

    private InputManager _im;

    private void Start()
    {
        _im = FindObjectOfType<InputManager>();
    }

    public void PositiveButtonOne()
    {
        EventManager.OnInteractionEvent(_im.GetSelectedAgent().transform.position, _positiveData1, new List<Agent>() { _im.GetSelectedAgent() });
    }

    public void PositiveButtonTwo()
    {
        EventManager.OnInteractionEvent(_im.GetSelectedAgent().transform.position, _positiveData2, new List<Agent>() { _im.GetSelectedAgent() });
    }

    public void NegativeButtonOne()
    {
        EventManager.OnInteractionEvent(_im.GetSelectedAgent().transform.position, _negativeData1, new List<Agent>() { _im.GetSelectedAgent() });
    }

    public void NegativeButtonTwo()
    {
        EventManager.OnInteractionEvent(_im.GetSelectedAgent().transform.position, _negativeData2, new List<Agent>() { _im.GetSelectedAgent() });
    }
}
