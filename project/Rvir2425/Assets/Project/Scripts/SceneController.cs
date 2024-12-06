using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadXRayScene()
    {
        SceneManager.LoadScene("XRayScene");
    }

    public void LoadCTScene()
    {
        SceneManager.LoadScene("CTScene");
    }   
    
}