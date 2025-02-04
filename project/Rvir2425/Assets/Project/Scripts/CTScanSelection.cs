using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class CTScanSelection : MonoBehaviour
{

    [System.Serializable]
    public class RegionsData
    {
        public ImageData[] images;
    }

    [System.Serializable]
    public class ImageData 
    {
        public string image_name;
        public int id;  // Important for CT scans
        public AlteredRegion altered_region;
    }

    [System.Serializable]
    public class AlteredRegion
    {
        public Center center;
        public float radius;
    }

    [System.Serializable]
    public class Center
    {
        public float x;
        public float y;
    }

    [Header("UI References")]

    [SerializeField] private RawImage displayImage1;   
    [SerializeField] private RawImage displayImage2;
    [SerializeField] private RawImage displayImage3;        

    [SerializeField] private GameObject CTScanSelectionPanel;  

    [SerializeField] private GameObject MainMenuPanel;  
    [SerializeField] private Button backButton;               // Reference to the Back button 
    [SerializeField] private GameObject imageViewerPanel;     // Reference to the image viewer panel directly

    [SerializeField] private GameObject imageInteractable;
    [SerializeField] private RawImage imageViewerDisplayImage;  
    [SerializeField] private Button backFromImageViewerButton;
    
    [SerializeField] private Slider imageSlider; // Back button in image viewer

    //TIMER
    [SerializeField] private Button stopTimeCTButton;

    private RegionsData regionsData;

    [Header("Photo Settings")]

    private Texture2D[] photoTextures;

    private Vector3 initialInteractablePosition;
    private Quaternion initialInteractableRotation;
    private Vector3 initialInteractableScale;

    [SerializeField] private ToggleController toggleController;  // Add this line


    //TIMER
    public int photoIndex;
    private TimeTracker timeTracker;
    private void Start()
    {
        Debug.Log("PhotoBrowser Start method called.");
        timeTracker = FindFirstObjectByType<TimeTracker>(); //TIMER
        AssignButtonListeners();
        SetupImagePanelAsButton(displayImage1, 1);
        SetupImagePanelAsButton(displayImage2, 2);
        SetupImagePanelAsButton(displayImage3, 3);
        TextAsset jsonFile = Resources.Load<TextAsset>("regions");
        regionsData = JsonUtility.FromJson<RegionsData>(jsonFile.text);
        stopTimeCTButton.gameObject.SetActive(true);
    
        initialInteractablePosition = imageInteractable.transform.position;
        initialInteractableRotation = imageInteractable.transform.rotation;
        initialInteractableScale = imageInteractable.transform.localScale;
    }

    private void SetupImagePanelAsButton(RawImage image, int photoIndex)
    {
        EventTrigger trigger = image.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ShowInImageViewer(photoIndex); });

        trigger.triggers.Add(entry);
    }


    private void AssignButtonListeners()
    {

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseSelectionPanel);
            Debug.Log("Back button OnClick listener assigned.");
        }
        else
        {
            Debug.LogError("BackButton is not assigned.");
        }


        if (backFromImageViewerButton != null)
        {
            backFromImageViewerButton.onClick.AddListener(CloseImageViewer);
            Debug.Log("Back from Image Viewer button OnClick listener assigned.");
        }

        if (stopTimeCTButton != null) //TIMER
        {
            stopTimeCTButton.onClick.AddListener(StopTrackingTime);
            Debug.Log("Back from Image Viewer button OnClick listener assigned.");
        }
    }

    public void CloseSelectionPanel()
    {
        Debug.Log("Closing selection panel.");
        if (CTScanSelectionPanel != null)
        {
            CTScanSelectionPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("ChooseXray Panel or Selection Panel is not assigned.");
        }
    }


private void ShowInImageViewer(int photo)
{
    
    Debug.LogError("Viewing image in image viewer." + photo);
    if (imageViewerDisplayImage != null)
    {
        string folderPath = "Photos/CTScan/CT" + photo;
        Texture2D[] textures = Resources.LoadAll<Texture2D>(folderPath);

        if (textures.Length == 0)
        {
            Debug.LogError("No images found in folder: " + folderPath);
            return;
        }
        Debug.Log($"Loaded textures ({textures.Length}):");
        foreach (var tex in textures)
        {
            Debug.Log($"- {tex.name}");
        }

        // Sort textures by name
        var sortedTextures = textures.OrderBy(t => t.name).ToArray();

        // Set the initial display image to the first image in the sorted list
        Texture2D initialTexture = sortedTextures[0];
        imageViewerDisplayImage.texture = initialTexture;

        float aspectRatio = (float)initialTexture.width / initialTexture.height;
        imageViewerDisplayImage.GetComponent<RectTransform>().sizeDelta =
            new Vector2(imageViewerDisplayImage.rectTransform.sizeDelta.x,
                    imageViewerDisplayImage.rectTransform.sizeDelta.x / aspectRatio);

        // Set slider properties based on the number of images
        SetSliderProperties(1, sortedTextures.Length);

        // Add listener to update the displayed image based on the slider value
        imageSlider.onValueChanged.RemoveAllListeners();
        imageSlider.onValueChanged.AddListener((value) => UpdateDisplayedImage(sortedTextures, (int)value));

        // Add this line before showing the image viewer panel
        if (toggleController != null)
        {
            toggleController.OnImageSelected();
        }
        photoIndex = photo;
        CTScanSelectionPanel.SetActive(false);
        imageViewerPanel.SetActive(true);
        timeTracker.StartTracking(photo, "CT");  //TIMER
        PositionStopButton(1);
    }
}

private void UpdateDisplayedImage(Texture2D[] sortedTextures, int index)
{
    if (index < 1 || index > sortedTextures.Length)
    {
        Debug.LogError("Invalid slider index");
        return;
    }

    Texture2D selectedTexture = sortedTextures[index - 1];
    imageViewerDisplayImage.texture = selectedTexture;

    float aspectRatio = (float)selectedTexture.width / selectedTexture.height;
    imageViewerDisplayImage.GetComponent<RectTransform>().sizeDelta =
        new Vector2(imageViewerDisplayImage.rectTransform.sizeDelta.x,
                   imageViewerDisplayImage.rectTransform.sizeDelta.x / aspectRatio);
    PositionStopButton(index);
    
}

    private void SetSliderProperties(int minValue, int maxValue)
    {
        if (imageSlider != null)
        {
            imageSlider.minValue = minValue;
            imageSlider.maxValue = maxValue;
            imageSlider.wholeNumbers = true;
        }
    }

    public void StopTrackingTime()     //TIMER
    {
        timeTracker.StopTracking();
        CloseImageViewer();
    }

    private void CloseImageViewer()
    {
        imageViewerPanel.SetActive(false);
        CTScanSelectionPanel.SetActive(true);

        imageInteractable.transform.position = initialInteractablePosition;
        imageInteractable.transform.rotation = initialInteractableRotation;
        imageInteractable.transform.localScale = initialInteractableScale;

    }

    private void PositionStopButton(int sliceId)
    {
        Debug.Log($"Positioning stop button for slice ID: {sliceId} on image CT{photoIndex}");
        String temp = $"CT{photoIndex}.png";  // photoIndex should be 1,2,3 for CT1,CT2,CT3
        ImageData imageData = System.Array.Find(regionsData.images, 
            img => img.image_name == temp);
        Debug.Log(imageData);
        if (imageData != null && imageData.id == sliceId)  // Only position button if slice ID matches
        {

            RectTransform buttonTransform = stopTimeCTButton.GetComponent<RectTransform>();

            buttonTransform.anchorMin = new Vector2(0.5f, 0.5f);
            buttonTransform.anchorMax = new Vector2(0.5f, 0.5f);
            buttonTransform.pivot = new Vector2(0.5f, 0.5f);

            buttonTransform.anchoredPosition = new Vector2(
                imageData.altered_region.center.x,
                imageData.altered_region.center.y
            );
            stopTimeCTButton.gameObject.SetActive(true);
            Debug.Log($"Button position set in CT{photoIndex}.png");
        }
        else
        {
            stopTimeCTButton.gameObject.SetActive(false);  // Hide button if not correct slice
        }
    }


}
