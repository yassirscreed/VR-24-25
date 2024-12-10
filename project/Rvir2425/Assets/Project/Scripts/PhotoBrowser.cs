using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhotoBrowser : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RawImage displayImage;           // RawImage component to display selected photo
    [SerializeField] private GameObject selectionPanel;    // Add this back
    [SerializeField] private TMP_Dropdown photoDropdown;      // Dropdown for selecting photos
    [SerializeField] private Button selectButton;             // Reference to the Select button
    [SerializeField] private Button chooseButton;             // Reference to the Choose button
    [SerializeField] private Button backButton;               // Reference to the Back button
    [SerializeField] private GameObject chooseXrayPanel;   // Add this back

    [Header("Photo Settings")]
    [SerializeField] private string photoFolderPath = "Photos/XRay"; // Folder name inside Resources folder (CTScan / XRay)

    private Texture2D[] photoTextures;

    private void Start()
    {
        Debug.Log("PhotoBrowser Start method called.");
        LoadPhotosFromResources();
        PopulateDropdown();
        SetupXRSelectListener();
        AssignButtonListeners();
    }

    private void LoadPhotosFromResources()
    {
        Debug.Log("Loading photos from Resources/" + photoFolderPath);
        // Load all texture files from Resources/Photos/XRay or CTScan folder
        photoTextures = Resources.LoadAll<Texture2D>(photoFolderPath);

        if (photoTextures.Length == 0)
        {
            Debug.LogWarning("No photos found in Resources/" + photoFolderPath);
        }
        else
        {
            Debug.Log(photoTextures.Length + " photos loaded from Resources/" + photoFolderPath);
        }
    }

    private void PopulateDropdown()
    {
        if (photoDropdown == null)
        {
            Debug.LogError("PhotoDropdown is not assigned.");
            return;
        }

        photoDropdown.ClearOptions();
        var options = photoTextures.Select(tex => tex.name).ToList();
        photoDropdown.AddOptions(options);
        Debug.Log("Dropdown populated with photo names.");
    }

    private void SetupXRSelectListener()
    {
        XRDirectInteractor interactor = GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {
            interactor.selectEntered.AddListener(OnSelectEntered);
            Debug.Log("XRSelect listener added.");
        }
        else
        {
            Debug.LogError("XRDirectInteractor component not found.");
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("Select action triggered by XR. Opening selection panel.");
        OpenSelectionPanel();
    }

    private void AssignButtonListeners()
    {
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(OpenSelectionPanel);
            Debug.Log("Select button OnClick listener assigned.");
        }
        else
        {
            Debug.LogError("SelectButton is not assigned.");
        }

        if (chooseButton != null)
        {
            chooseButton.onClick.AddListener(OnPhotoSelected);
            Debug.Log("Choose button OnClick listener assigned.");
        }
        else
        {
            Debug.LogError("ChooseButton is not assigned.");
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseSelectionPanel);
            Debug.Log("Back button OnClick listener assigned.");
        }
        else
        {
            Debug.LogError("BackButton is not assigned.");
        }
    }

    // Method to open the selection panel via UIManager
    public void OpenSelectionPanel()
    {
        Debug.Log("Opening selection panel.");
        if (chooseXrayPanel != null && selectionPanel != null)
        {
            chooseXrayPanel.SetActive(false);
            selectionPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("ChooseXray Panel or Selection Panel is not assigned.");
        }
    }

    // Method to close the selection panel via UIManager
    public void CloseSelectionPanel()
    {
        Debug.Log("Closing selection panel.");
        if (chooseXrayPanel != null && selectionPanel != null)
        {
            selectionPanel.SetActive(false);
            chooseXrayPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("ChooseXray Panel or Selection Panel is not assigned.");
        }
    }

    // Method to handle photo selection
    public void OnPhotoSelected()
    {
        if (photoDropdown == null)
        {
            Debug.LogError("PhotoDropdown is not assigned.");
            return;
        }

        int selectedIndex = photoDropdown.value;
        if (selectedIndex < 0 || selectedIndex >= photoTextures.Length)
        {
            Debug.LogWarning("Invalid photo selection index.");
            return;
        }

        Texture2D selectedTexture = photoTextures[selectedIndex];
        Debug.Log("Photo selected from dropdown: " + selectedTexture.name);
        DisplaySelectedPhoto(selectedTexture);

        // Close the selection panel after selection
        CloseSelectionPanel();
    }

    private void DisplaySelectedPhoto(Texture2D selectedTexture)
    {
        if (displayImage != null && selectedTexture != null)
        {
            displayImage.texture = selectedTexture;

            // Maintain aspect ratio
            float aspectRatio = (float)selectedTexture.width / selectedTexture.height;
            displayImage.GetComponent<RectTransform>().sizeDelta =
                new Vector2(displayImage.rectTransform.sizeDelta.x,
                           displayImage.rectTransform.sizeDelta.x / aspectRatio);
            Debug.Log("Displayed photo: " + selectedTexture.name + " with aspect ratio: " + aspectRatio);
        }
        else
        {
            Debug.LogWarning("DisplayImage or selectedTexture is null.");
        }
    }
}
