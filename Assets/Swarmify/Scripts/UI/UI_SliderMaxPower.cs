public class UI_SliderMaxPower : AUI_Slider
{
    public static event SliderChangedDelegate MaxPowerChangedEvent;

    protected override void OnSliderValueChanged(float value)
    {
        base.OnSliderValueChanged(value);

        if (MaxPowerChangedEvent != null)
        {
            MaxPowerChangedEvent(value);
        }
    }
}