using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralData;

public class StatsGUI : MonoBehaviour
{
    [SerializeField] private List<StatSliderUI> _stats = new(Data.NUMBER_OF_STATS);
    [SerializeField] private StatSliderUI _playerRelation;
    [SerializeField] private GameObject _panel;

    private WaitForSeconds _update = new(Data.UPDATE_REFRESH_IN_SEC);

    private delegate IEnumerator PlayerRelationUpdate(Agent agent);
    private PlayerRelationUpdate _playerRelationUpdateFunc;
    private Coroutine _playerRelationCoroutine;

    private void Awake()
    {
        TurnPanelOff();
        _playerRelationUpdateFunc = UpdateRelation;
    }

    private void Start()
    {
        EventManager.AgentSelected += DisplaySelectedAgentStats;
        EventManager.Deselect += TurnPanelOff;
    }

    private void OnDestroy()
    {
        EventManager.AgentSelected -= DisplaySelectedAgentStats;
        EventManager.Deselect -= TurnPanelOff;
    }

    private void DisplaySelectedAgentStats(Agent agent)
    {
        Personality personality = agent.Personality;
        TogglePanel(true);
        StartPlayerRelationUpdate(agent);

        foreach (StatSliderUI stat in _stats)
        {
            switch (stat.Stat)
            {
                case StatType.OPENNESS:
                    stat.UpdateSlider(personality.Openness);
                    break;
                case StatType.CONSCIENTIOUSNESS:
                    stat.UpdateSlider(personality.Conscientiousness);
                    break;
                case StatType.EXTRAVERSION:
                    stat.UpdateSlider(personality.Extraversion);
                    break;
                case StatType.AGREEABLENESS:
                    stat.UpdateSlider(personality.Agreementableness);
                    break;
                case StatType.NEUROTICISM:
                    stat.UpdateSlider(personality.Neuroticism);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void TurnPanelOff() => TogglePanel(false);

    private void TogglePanel(bool value)
    {
        _panel.SetActive(value);
    }

    private void StartPlayerRelationUpdate(Agent agent)
    {
        if (_playerRelationCoroutine != null)
        {
            StopCoroutine(_playerRelationCoroutine);
        }
        _playerRelationCoroutine = StartCoroutine(_playerRelationUpdateFunc(agent));
    }

    private IEnumerator UpdateRelation(Agent agent)
    {
        while (_panel.activeSelf)
        {
            yield return _update;
            _playerRelation.UpdateSlider(agent.RelationToPlayer);
        }

        _playerRelationCoroutine = null;
        yield break;
    }
}
