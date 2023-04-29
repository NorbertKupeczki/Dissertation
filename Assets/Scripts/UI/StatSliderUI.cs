using GeneralData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSliderUI : MonoBehaviour
{
    [SerializeField] private StatType _stat;
    private Slider _slider;
    private TextMeshProUGUI _valueText;

    public StatType Stat => _stat;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _slider.maxValue = Data.MAX_STAT_VALUE;

        _valueText = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    /// <summary>
    /// Updates the slider based on the provided parameter.
    /// </summary>
    /// <param name="value"></param>
    public void UpdateSlider(int value)
    {
        _valueText.text = value.ToString();
        _slider.value = value;
    }
}
