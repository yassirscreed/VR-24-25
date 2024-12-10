using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] public GameObject mainMenuPanel;
    [SerializeField] private GameObject chooseXrayPanel;
    [SerializeField] private GameObject chooseCTScanPanel;
    [SerializeField] private GameObject helpPanel;

    [Header("Buttons")]
    [SerializeField] private Button viewXrayButton;
    [SerializeField] private Button viewCTScanButton;
    [SerializeField] private Button backFromChooseXrayButton;
    [SerializeField] private Button backFromChooseCTScanButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button backFromHelpButton;

    private void Start()
    {
        if (viewXrayButton != null)
            viewXrayButton.onClick.AddListener(ShowChooseXrayPanel);
        if (viewCTScanButton != null)
            viewCTScanButton.onClick.AddListener(ShowChooseCTScanPanel);
        if (backFromChooseXrayButton != null)
            backFromChooseXrayButton.onClick.AddListener(ShowMainMenu);
        if (backFromChooseCTScanButton != null)
            backFromChooseCTScanButton.onClick.AddListener(ShowMainMenu);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitApplication);
        if (helpButton != null)
            helpButton.onClick.AddListener(ShowHelpPanel);
        if (backFromHelpButton != null)
            backFromHelpButton.onClick.AddListener(ShowMainMenu);

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        chooseXrayPanel.SetActive(false);
        chooseCTScanPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void ShowChooseXrayPanel()
    {
        mainMenuPanel.SetActive(false);
        chooseXrayPanel.SetActive(true);
        chooseCTScanPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void ShowChooseCTScanPanel()
    {
        mainMenuPanel.SetActive(false);
        chooseXrayPanel.SetActive(false);
        chooseCTScanPanel.SetActive(true);
        helpPanel.SetActive(false);
    }

    public void ShowHelpPanel()
    {
        mainMenuPanel.SetActive(false);
        chooseXrayPanel.SetActive(false);
        chooseCTScanPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
