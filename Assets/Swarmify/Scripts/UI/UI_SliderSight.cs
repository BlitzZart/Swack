public class UI_SliderSight : AUI_Slider
{
    public static event SliderChangedDelegate SightDistanceChangedEvent;

    protected override void OnSliderValueChanged(float value)
    {
        base.OnSliderValueChanged(value);

        if (SightDistanceChangedEvent != null)
        {
            SightDistanceChangedEvent(value);
        }
    }
}
