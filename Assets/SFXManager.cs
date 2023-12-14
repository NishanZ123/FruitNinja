using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public Button[] buttons; // All the buttons to assign for clicking sound

    void Start()
    {
        AssignButtonEvents();
    }

    private void AssignButtonEvents()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager instance not found.");
            return;
        }

        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(audioManager.PlayClickSound);

            
            switch (button.name)
            {
                case "StartButton":
                    button.onClick.AddListener(() => LoadScene("Test")); // used for testing but keep
                    break;
                case "QuitButton":
                    button.onClick.AddListener(Application.Quit);
                    break;
                    
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

