using System.IO;
using Meta.Voice.Logging;
using UnityEngine;
using UnityEngine.UI;

public class UniversalButtonLogger : MonoBehaviour
{
    public string logFileName = "buttonInteractLogs.csv";
    private string logFilePath;
    private int logSize = 0;

    private void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, logFileName);
        string dateTimeNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(logFilePath, $"Log {dateTimeNow}\n");
        File.AppendAllText(logFilePath, "Index,Time(ms),Event,ButtonName\n");
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        Debug.Log($"Found {allButtons.Length} buttons in the scene.");

        foreach (Button button in allButtons)
        {
            string buttonName = button.gameObject.name;

            button.onClick.AddListener(() => LogButtonClick(buttonName));
            Debug.Log($"Added click listener for button: {buttonName}");
        }
    }

    private void LogButtonClick(string buttonName)
    {
        WriteLog($",{Time.time * 1000},Click,{buttonName}");
        Debug.Log($"Button clicked: {buttonName}");
    }

    private void WriteLog(string logEntry)
    {
        try
        {
            File.AppendAllText(logFilePath, logSize.ToString() + logEntry + "\n");
            logSize++;
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to write log: " + ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log($"Log file saved at: {logFilePath}");
    }
}
