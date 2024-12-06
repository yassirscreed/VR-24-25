using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Logger : MonoBehaviour
{
    public string logFileName = "eventLogs.csv";
    private string logFilePath;
    private int logSize = 0;

    [SerializeField]
    private XRRayInteractor leftRayInteractor;
    [SerializeField]
    private XRRayInteractor rightRayInteractor;

    private void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, logFileName);
        WriteLog(",Time(ms),Event,ButtonName,Controller");
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        Transform buttonsParent = GameObject.Find("buttons")?.transform;
        if (buttonsParent != null)
        {
            foreach (Transform buttonTransform in buttonsParent)
            {
                var button = buttonTransform.GetComponentInChildren<Button>();
                if (button != null)
                {
                    var interactable = button.GetComponent<XRSimpleInteractable>();
                    if (interactable == null)
                    {
                        interactable = button.gameObject.AddComponent<XRSimpleInteractable>();
                    }

                    interactable.hoverEntered.AddListener((HoverEnterEventArgs args) =>
                    {
                        LogButtonEvent(button.name, "HoverEnter", args.interactorObject);
                    });

                    interactable.hoverExited.AddListener((HoverExitEventArgs args) =>
                    {
                        LogButtonEvent(button.name, "HoverExit", args.interactorObject);
                    });

                    interactable.selectEntered.AddListener((SelectEnterEventArgs args) =>
                    {
                        LogButtonEvent(button.name, "Click", args.interactorObject);
                    });
                }
            }
        }
        else
        {
            Debug.LogWarning("Buttons parent object not found!");
        }
    }

    private void LogButtonEvent(string buttonName, string eventType, IXRInteractor interactor)
    {
        string controllerType = "Unknown";
        if (interactor is XRRayInteractor rayInteractor)
        {
            if (rayInteractor == leftRayInteractor)
                controllerType = "LeftController";
            else if (rayInteractor == rightRayInteractor)
                controllerType = "RightController";
        }

        WriteLog($",{Time.time * 1000},{eventType},{buttonName},{controllerType}");
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