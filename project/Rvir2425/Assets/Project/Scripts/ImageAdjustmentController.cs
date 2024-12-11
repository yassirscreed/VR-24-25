using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ImageAdjustmentController : MonoBehaviour
{
    [SerializeField] private RawImage targetImage;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider contrastSlider;

    private Material imageMaterial;
    private static readonly int BrightnessProperty = Shader.PropertyToID("_Brightness");
    private static readonly int ContrastProperty = Shader.PropertyToID("_Contrast");

    private void Start()
    {
        // Create a new material instance using the shader
        var shader = Shader.Find("Custom/ImageAdjustment");
        if (shader == null)
        {
            Debug.LogError("Could not find Custom/ImageAdjustment shader. Make sure the shader is created correctly.");
            return;
        }

        imageMaterial = new Material(shader);
        
        // Assign the material to the RawImage
        if (targetImage != null)
        {
            targetImage.material = imageMaterial;
            // Preserve the current texture
            imageMaterial.mainTexture = targetImage.texture;
        }
        else
        {
            Debug.LogError("Target Image is not assigned!");
            return;
        }

        // Configure and add listeners to sliders with more subtle ranges
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = -0.5f;  // Reduced from -1
            brightnessSlider.maxValue = 0.5f;   // Reduced from 1
            brightnessSlider.value = 0f;
            brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
        }
        else
        {
            Debug.LogError("Brightness Slider is not assigned!");
        }

        if (contrastSlider != null)
        {
            contrastSlider.minValue = 0.5f;     // Changed from 0
            contrastSlider.maxValue = 1.5f;     // Reduced from 2
            contrastSlider.value = 1f;
            contrastSlider.onValueChanged.AddListener(UpdateContrast);
        }
        else
        {
            Debug.LogError("Contrast Slider is not assigned!");
        }

        // Set initial values
        UpdateBrightness(brightnessSlider.value);
        UpdateContrast(contrastSlider.value);
    }

    private void UpdateBrightness(float value)
    {
        if (imageMaterial != null)
        {
            imageMaterial.SetFloat(BrightnessProperty, value);
        }
    }

    private void UpdateContrast(float value)
    {
        if (imageMaterial != null)
        {
            imageMaterial.SetFloat(ContrastProperty, value);
        }
    }

    private void OnDestroy()
    {
        // Clean up the material when the object is destroyed
        if (imageMaterial != null)
        {
            Destroy(imageMaterial);
        }
    }
}