public class UI_SliderMaxSpeed : AUI_Slider
{
    public static event SliderChangedDelegate MaxSpeedChangedEvent;

    protected override void OnSliderValueChanged(float value)
    {
        base.OnSliderValueChanged(value);

        if (MaxSpeedChangedEvent != null)
        {
            MaxSpeedChangedEvent(value);
        }
    }
}