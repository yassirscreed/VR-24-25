using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

[System.Serializable]
public class RegionsData
{
    public ImageData[] images;
}

[System.Serializable]
public class ImageData 
{
    public string image_name;
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

public class XRaySelection : MonoBehaviour
{
    [Header("UI References")]

    [SerializeField] private RawImage displayImage1;   
    [SerializeField] private RawImage displayImage2;
    [SerializeField] private RawImage displayImage3;        

    [SerializeField] private GameObject XRaySelectionPanel;  

    [SerializeField] private GameObject MainMenuPanel;  
    [SerializeField] private Button backButton;               // Reference to the Back button 
    [SerializeField] private GameObject imageViewerPanel; 
    
    [SerializeField] private GameObject imageInteractable;    // Reference to the image viewer panel directly
    [SerializeField] private RawImage imageViewerDisplayImage;  
    [SerializeField] private Button backFromImageViewerButton; // Back button in image viewer

    //TIMER
     [SerializeField] private Button stopTimeXRButton;



    [Header("Photo Settings")]
    private Texture2D[] photoTextures;
    private Vector3 initialInteractablePosition;
    private Quaternion initialInteractableRotation;
    private Vector3 initialInteractableScale;
    
    //TIMER
    public int photoIndex;
    private TimeTracker timeTracker;

    //REGION
    private RegionsData regionsData;

    [SerializeField] private ToggleController toggleController;

    private void Start()
    {

        Debug.Log("PhotoBrowser Start method called.");
        timeTracker = FindFirstObjectByType<TimeTracker>(); //TIMER
        AssignButtonListeners();
        SetupImagePanelAsButton(displayImage1, 1);
        SetupImagePanelAsButton(displayImage2, 2);
        SetupImagePanelAsButton(displayImage3, 3);
        initialInteractablePosition = imageInteractable.transform.position;
        initialInteractableRotation = imageInteractable.transform.rotation;
        initialInteractableScale = imageInteractable.transform.localScale;
        string path = Path.Combine(Application.dataPath, "Resources/regions.json");
        TextAsset jsonFile = Resources.Load<TextAsset>("regions");
        regionsData = JsonUtility.FromJson<RegionsData>(jsonFile.text);
    }

     private void SetupImagePanelAsButton(RawImage image, int photoIndex)
    {
        EventTrigger trigger = image.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ShowInImageViewer(photoIndex); });

        trigger.triggers.Add(entry);
    }

    private void PositionStopButton(int imageName)
    {
        String temp =  $"XR{imageName}.png";
        ImageData imageData = System.Array.Find(regionsData.images, 
            img => img.image_name == temp);
        
        if (imageData != null)
        {
            RectTransform buttonTransform = stopTimeXRButton.GetComponent<RectTransform>();

            // Setup button's RectTransform properties
            buttonTransform.anchorMin = new Vector2(0.5f, 0.5f);
            buttonTransform.anchorMax = new Vector2(0.5f, 0.5f);
            buttonTransform.pivot = new Vector2(0.5f, 0.5f);

            // Set position directly from regions file
            buttonTransform.anchoredPosition = new Vector2(
                imageData.altered_region.center.x,
                imageData.altered_region.center.y
            );

        }
        else
        {
            Debug.LogError($"No data found for image {temp}");
        }
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

        if (stopTimeXRButton != null) //TIMER
        {
            stopTimeXRButton.onClick.AddListener(StopTrackingTime);

            Debug.Log("Back from Image Viewer button OnClick listener assigned.");
        }
    }

    public void CloseSelectionPanel()
    {
        Debug.Log("Closing selection panel.");
        XRaySelectionPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
      
        
        
    }


    private void ShowInImageViewer(int photo)
    {

        if (imageViewerDisplayImage != null)
        {
            RawImage displayImage = null;
            switch (photo)
            {
                case 1:
                    displayImage = displayImage1;
                    break;
                case 2:
                    displayImage = displayImage2;
                    break;
                case 3:
                    displayImage = displayImage3;
                    break;
                default:
                    Debug.LogError("Invalid photo index");
                    return;
            }

            imageViewerDisplayImage.texture = displayImage.texture;
            float aspectRatio = (float)displayImage.texture.width / displayImage.texture.height;
            imageViewerDisplayImage.GetComponent<RectTransform>().sizeDelta =
                new Vector2(imageViewerDisplayImage.rectTransform.sizeDelta.x,
                           imageViewerDisplayImage.rectTransform.sizeDelta.x / aspectRatio);

            if (toggleController != null)
            {
                toggleController.OnImageSelected();
            }
        }

        XRaySelectionPanel.SetActive(false);
        imageViewerPanel.SetActive(true);
        timeTracker.StartTracking(photo, "XR");  //TIMER
        PositionStopButton(photo);
    }

    public void StopTrackingTime()     //TIMER
    {
        timeTracker.StopTracking();
        CloseImageViewer();
    }

    private void CloseImageViewer()
    {
        imageViewerPanel.SetActive(false);
        XRaySelectionPanel.SetActive(true);
        imageInteractable.transform.position = initialInteractablePosition;
        imageInteractable.transform.rotation = initialInteractableRotation;
        imageInteractable.transform.localScale = initialInteractableScale;
    }


}
