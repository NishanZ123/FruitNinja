using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Change to UIManager type
    public Text creditsText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        InitializeCreditsDisplay();
    }

    public void UpdateCreditsDisplay(int credits) // Ensure this method is public
    {
        creditsText.text = "Credits: " + credits;
    }

    private void InitializeCreditsDisplay()
    {
        if (creditsText != null)
        {
            creditsText.text = "Credits: " + Ninja_Player.Instance.credits;
        }
    }
}


