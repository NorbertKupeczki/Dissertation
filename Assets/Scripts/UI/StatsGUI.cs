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

    private Agent _selectedAgent;

    private void Awake()
    {
        TurnPanelOff();
        _playerRelationUpdateFunc = UpdateRelation;
    }

    private void Start()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

#region >> Event subscription functions
    private void SubscribeEvents()
    {
        EventManager.AgentSelected += DisplaySelectedAgentStats;
        EventManager.Deselect += TurnPanelOff;
    }

    private void UnsubscribeEvents()
    {
        EventManager.AgentSelected -= DisplaySelectedAgentStats;
        EventManager.Deselect -= TurnPanelOff;
    }
#endregion

    /// <summary>
    /// Turns on the display panel and loads sets the values to match the selected agent's statistics.
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void DisplaySelectedAgentStats(Agent agent)
    {
        Personality personality = agent.Personality;
        _selectedAgent = agent;
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

    /// <summary>
    /// Turns the panel off
    /// </summary>
    private void TurnPanelOff() => TogglePanel(false);

    /// <summary>
    /// Toggles the visibility of the panel
    /// </summary>
    /// <param name="value"></param>
    private void TogglePanel(bool value)
    {
        _panel.SetActive(value);
        if (!value) _selectedAgent = null;
    }

    /// <summary>
    /// Starts the updating of the selected agent's player relation bar.
    /// </summary>
    /// <param name="agent"></param>
    private void StartPlayerRelationUpdate(Agent agent)
    {
        if (_playerRelationCoroutine != null)
        {
            StopCoroutine(_playerRelationCoroutine);
        }
        _playerRelationCoroutine = StartCoroutine(_playerRelationUpdateFunc(agent));
    }

    /// <summary>
    /// Updates the selected agent's player relationship data
    /// </summary>
    /// <param name="agent"></param>
    /// <returns>IEnumerator</returns>
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
