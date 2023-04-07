using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personality
{
    public Personality()
    {
        RandomGeneratePersonality();
    }

    private float _openness; // inventive/curious vs. consistent/cautious
    private float _conscientiousness; // efficient/organized vs. extravagant/careless
    private float _extraversion; // outgoing/energetic vs. solitary/reserved
    private float _agreeableness; // friendly/compassionate vs. critical/rational
    private float _neuroticism; // sensitive/nervous vs. resilient/confident

    #region >> Getters
    public float GetOpenness() => _openness;

    public float GetConscientiousness() => _conscientiousness;

    public float GetExtraversion() => _extraversion;

    public float GetAgreementableness() => _agreeableness;

    public float GetNeuroticism() => _neuroticism;
    #endregion

    #region >> Setters
    public void SetOpenness(float value) => _openness = value;

    public void SetConscientiousness(float value) => _conscientiousness = value;

    public void SetExraversion(float value) => _extraversion = value;

    public void SetAgreeableness(float value) => _agreeableness = value;

    public void SetNeuroticism(float value) => _neuroticism = value;
    #endregion

    public void RandomGeneratePersonality()
    {
        _openness = GetRandomTrait();
        _conscientiousness = GetRandomTrait();
        _extraversion = GetRandomTrait();
        _agreeableness = GetRandomTrait();
        _neuroticism = GetRandomTrait();
    }

    private float GetRandomTrait()
    {
        return (Random.Range(0, 6) + Random.Range(0, 6)) / 10f;
    }

    public void PrintTraits()
    {
        Debug.Log("O: " + _openness + " | C: " + _conscientiousness + " | E: " + _extraversion + " | A: " + _agreeableness + " | N: " + _neuroticism);
    }
}
