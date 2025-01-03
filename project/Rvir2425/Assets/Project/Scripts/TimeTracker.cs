using System.IO;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    private float startTime;
    private bool isTracking = false;
    private string logFilePath;
    private static int imageId = 0;
     private int logSize = 0;
    private static string _type; // Set your image ID statically here

    void Start()
    {
        logFilePath = Path.Combine("/sdcard/Download/", "TimeLog.txt");
        string dateTimeNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(logFilePath, $"Log {dateTimeNow}\n");
    }

    public void StartTracking(int id, string type)
    {
        startTime = Time.realtimeSinceStartup;
        isTracking = true;
        _type = type;
        imageId = id;
    }

    public void StopTracking()
    {
        if (isTracking)
        {
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            string entry = $"{_type}{imageId}\t{elapsedTime:F3} seconds";
            WriteLog(entry);
            isTracking = false;
        }
    }


    private void WriteLog(string logEntry)
    {
        try
        {
            File.AppendAllText(logFilePath,logEntry + "\n");
            logSize++;
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to write log: " + ex.Message);
        }
    }

}