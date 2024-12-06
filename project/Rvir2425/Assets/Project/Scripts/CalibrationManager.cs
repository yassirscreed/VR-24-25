using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CalibrationManager : MonoBehaviour
{
    private OVRCameraRig cameraRig;

    void Start()
    {
        // Find the OVRCameraRig in the scene
        cameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        if (cameraRig == null)
        {
            Debug.LogError("No OVRCameraRig found in scene!");
            return;
        }
    }

    void Update()
    {
        foreach (OVRInput.Button button in System.Enum.GetValues(typeof(OVRInput.Button)))
        {
            if (OVRInput.GetDown(button))
            {
                SceneManager.LoadScene("MainMenuScene");
                break;
            }
        }
    }
}
