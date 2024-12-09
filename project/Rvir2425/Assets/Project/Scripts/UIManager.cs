using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;         // Main Menu Canvas
    [SerializeField] private GameObject chooseXrayPanel;       // ChooseXray Panel
    [SerializeField] private GameObject chooseCTScanPanel;    // ChooseCTScan Panel
    [SerializeField] private GameObject selectionPanel;        // Selection Panel

    [Header("Buttons")]
    [SerializeField] private Button viewXrayButton;            // "View Xray" button on Main Menu
    [SerializeField] private Button viewCTScanButton;             // "View CT Scan" button on Main Menu
    [SerializeField] private Button backFromChooseXrayButton;     // "Back" button on ChooseXray Panel
    [SerializeField] private Button backFromChooseCTScanButton;   // "Back" button on ChooseCTScan Panel

    private string currentContext = ""; // To track whether in XRay or CTScan context

    private void Start()
    {
        // Assign button listeners
        if (viewXrayButton != null)
            viewXrayButton.onClick.AddListener(ShowChooseXrayPanel);
        else
            Debug.LogError("View Xray Button is not assigned in UIManager.");

        if (viewCTScanButton != null)
            viewCTScanButton.onClick.AddListener(ShowChooseCTScanPanel);
        else
            Debug.LogError("View CT Scan Button is not assigned in UIManager.");

        if (backFromChooseXrayButton != null)
            backFromChooseXrayButton.onClick.AddListener(ShowMainMenu);
        else
            Debug.LogError("Back Button on ChooseXray Panel is not assigned in UIManager.");

        if (backFromChooseCTScanButton != null)
            backFromChooseCTScanButton.onClick.AddListener(ShowMainMenu);
        else
            Debug.LogError("Back Button on ChooseCTScan Panel is not assigned in UIManager.");

        // Initialize panels
        ShowMainMenu();
    }

    // Show Main Menu and hide all other panels
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        chooseXrayPanel.SetActive(false);
        chooseCTScanPanel.SetActive(false);
        selectionPanel.SetActive(false);
        currentContext = "";
        Debug.Log("Main Menu is now active.");
    }

    // Show ChooseXray Panel and hide others
    public void ShowChooseXrayPanel()
    {
        mainMenuPanel.SetActive(false);
        chooseXrayPanel.SetActive(true);
        chooseCTScanPanel.SetActive(false);
        selectionPanel.SetActive(false);
        currentContext = "XRay";
        Debug.Log("ChooseXray Panel is now active.");
    }

    // Show ChooseCTScan Panel and hide others
    public void ShowChooseCTScanPanel()
    {
        mainMenuPanel.SetActive(false);
        chooseXrayPanel.SetActive(false);
        chooseCTScanPanel.SetActive(true);
        selectionPanel.SetActive(false);
        currentContext = "CTScan";
        Debug.Log("ChooseCTScan Panel is now active.");
    }

    // Show Selection Panel from Choose Panels
    public void ShowSelectionPanel()
    {
        if (currentContext == "XRay")
        {
            chooseXrayPanel.SetActive(false);
        }
        else if (currentContext == "CTScan")
        {
            chooseCTScanPanel.SetActive(false);
        }

        selectionPanel.SetActive(true);
        Debug.Log("Selection Panel is now active.");
    }

    // Hide Selection Panel and return to the respective Choose Panel
    public void HideSelectionPanel(string panelName)
    {
        selectionPanel.SetActive(false);
        if (panelName == "XRay")
        {
            chooseXrayPanel.SetActive(true);
            Debug.Log("Returned to ChooseXray Panel.");
        }
        else if (panelName == "CTScan")
        {
            chooseCTScanPanel.SetActive(true);
            Debug.Log("Returned to ChooseCTScan Panel.");
        }
        else
        {
            Debug.LogWarning("Unknown panel name provided to HideSelectionPanel.");
        }
    }
}
