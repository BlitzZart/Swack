using UnityEngine;
using UnityEngine.UI;

public abstract class AUI_Slider : MonoBehaviour
{
    public delegate void SliderChangedDelegate(float value);

    [Header("Initialization Values")]
    [Tooltip("Changes have no effect during runtime.")]
    [SerializeField]
    private float m_defaultValue = 0.0f;
    [SerializeField]
    private float m_minValue = 0.0f;
    [SerializeField]
    private float m_maxValue = 1.0f;
    [SerializeField]
    private string m_labelText;
    [SerializeField]
    private Slider.Direction m_direction;
    [SerializeField]
    private bool m_wholeNumbers;

    private Text m_txtLabel, m_txtValue;
    private Slider m_slider;

    #region unity callbacks
    private void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        m_txtLabel = texts[0];
        m_txtValue = texts[1];
        m_slider = GetComponentInChildren<Slider>();

        m_txtLabel.text = m_labelText;
        m_txtValue.text = m_defaultValue.ToString("0.00");

        m_slider.value = m_defaultValue;

        m_slider.minValue = m_minValue;
        m_slider.maxValue = m_maxValue;
        m_slider.direction = m_direction;
        m_slider.wholeNumbers = m_wholeNumbers;

        m_slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Destroy()
    {
        m_slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
    #endregion

    protected virtual void OnSliderValueChanged(float value)
    {
        m_txtValue.text = value.ToString("0.00");
    }
}
