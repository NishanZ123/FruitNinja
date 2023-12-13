using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public Button[] buttons; // Assign all interactive buttons in the scene

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

            // Customize button functionality based on the button's name or tag
            switch (button.name)
            {
                case "StartButton":
                    button.onClick.AddListener(() => LoadScene("GameSceneName")); // Replace with your game scene name
                    break;
                case "QuitButton":
                    button.onClick.AddListener(Application.Quit);
                    break;
                    // Add more cases for other buttons
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

