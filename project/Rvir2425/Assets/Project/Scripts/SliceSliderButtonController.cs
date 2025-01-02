using UnityEngine;
using UnityEngine.UI;

public class SliceSliderController : MonoBehaviour
{
    public Slider sliceSlider; // Assign your slice_slide slider in the Inspector
    public float step = 1f;    // Step size for increment/decrement

    public void IncreaseValue()
    {
        sliceSlider.value = Mathf.Min(sliceSlider.maxValue, sliceSlider.value + step);
    }

    public void DecreaseValue()
    {
        sliceSlider.value = Mathf.Max(sliceSlider.minValue, sliceSlider.value - step);
    }
}
