using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ninja_Player : MonoBehaviour
{
    public Spawn_items spawner;
    public GameObject gameOverPanel; // Assign this in the Inspector
    public int lives = 3; // Starting number of lives
    public static Ninja_Player Instance { get; private set; }
    private Vector3 pos;
    public int score = 0; //Score
    public int credits; // credits
    public Text slowMotionQuantityText;
    public Text doublePointsQuantityText;
    public Text extraLifeQuantityText;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.InitializeNinjaGameUI(slowMotionQuantityText, doublePointsQuantityText, extraLifeQuantityText);
        LoadCredits();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Instance == null)
        {
            Instance = this;
            // Other initialization code, if any
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensures only one instance exists
        }

            InventoryManager.Instance.OnInventoryChanged += UpdatePowerupDisplay;
            UpdatePowerupDisplay(); // Initial update
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // 'C' for 'Credit', can be any key you prefer
        {
            AddCredits(1000); // Adds 1000 credits each time 'C' is pressed
            SaveCredits(); // Save the new credits value
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGameData();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            UseSlowDownTimePowerUp();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            UseExtraLifePowerUp();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            UseDoublePointsPowerUp();
        }
        //If the game is running on an Android device
        if (Application.platform == RuntimePlatform.Android)
        {
            //If we are hitting the screen
            if (Input.touchCount == 1)
            {
                //Find screen touch position, by
                //transforming position from screen space into game world space.
                pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1));
                //Set position of the player object
                transform.position = new Vector3(pos.x, pos.y, 3);
                //Set collider to true
                GetComponent<Collider2D>().enabled = true;
                return;
            }
            //Set collider to false
            GetComponent<Collider2D>().enabled = false;
        }
        else //If the game is not running on an iPhone device
        {
            //Find mouse position
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            //Set position
            transform.position = new Vector3(pos.x, pos.y, 3);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Fruit2D item = other.gameObject.GetComponent<Fruit2D>();

        if (other.tag == "Fruit")
        {
            if (item != null)
            {
                item.Hit();

                // Check if double points power-up is active
                int pointsToAdd = PowerUpManager.Instance.IsDoublePointsActive ? 2 : 1;
                score += pointsToAdd;
                credits += pointsToAdd;

                Debug.Log(score);
                Debug.Log(credits);
            }
        }
        else if (other.tag == "Enemy")
        {
            if (item != null && item.isBomb)
            {
                item.Hit();
                LoseLife(); // Lose a life when hitting a bomb
                Debug.Log("Lives: " + lives);
            }
        }
    }


    public void IncrementScore(int points)
    {
        score += points;
        // Update UI or any other relevant components
    }
    public void AddCredits(int amount)
    {
        credits += amount;
        UIManager.Instance.UpdateCreditsDisplay(credits);
        SaveCredits();
    }

    public void EarnCredits(int amount)
    {
        AddCredits(amount); // Assuming this is how credits are earned
    }
    public void SaveCredits()
    {
        PlayerPrefs.SetInt("PlayerCredits", credits);
        PlayerPrefs.Save();
    }

    public void LoadCredits()
    {
        // Check if saved data exists
        if (PlayerPrefs.HasKey("PlayerCredits"))
        {
            // Load saved credits
            credits = PlayerPrefs.GetInt("PlayerCredits", 0);
        }
        else
        {
            // Set default credits to 1000 if no saved data exists
            credits = 1000;
        }

        UIManager.Instance.UpdateCreditsDisplay(credits);
    }
    private void ResetGameData()
    {
        // Reset credits
        credits = 0;
        SaveCredits();

        // Reset power-ups - assuming InventoryManager has a method to reset inventory
        InventoryManager.Instance.ResetInventory();
        InventoryManager.Instance.SaveInventory();

        // Update UI elements
        UIManager.Instance.UpdateCreditsDisplay(credits);
        InventoryManager.Instance.UpdateInventoryUI();
    }
    private void UseSlowDownTimePowerUp()
    {
        if (InventoryManager.Instance.GetPowerUpCount("SlowMotion") > 0)
        {
            PowerUpManager.Instance.ActivateSlowMotion(); // Activate the slowdown effect
            InventoryManager.Instance.RemovePowerUp("SlowMotion", 1);
        }
        else
        {
            Debug.Log("No Slowdown Power-ups left to use.");
        }
    }

    private void UseDoublePointsPowerUp()
    {
        if (InventoryManager.Instance.GetPowerUpCount("DoublePoints") > 0)
        {
            float duration = 10f; // Duration of double points effect
            PowerUpManager.Instance.ActivateDoublePoints(duration);

            InventoryManager.Instance.RemovePowerUp("DoublePoints", 1);
            InventoryManager.Instance.UpdateInventoryUI();
        }
    }

     public void LoseLife()
     {
        if (lives > 0)
        {
            lives--;
            // Update UI or any other relevant components

            if (lives <= 0)
            {
                GameOver();
            }
        }
     }
    
    
     public void GainLife()
     {
        lives++;
        // Update UI
     }

    private void UseExtraLifePowerUp()
    {
        if (InventoryManager.Instance.GetPowerUpCount("ExtraLife") > 0)
        {
            GainLife();
            InventoryManager.Instance.RemovePowerUp("ExtraLife", 1);
            InventoryManager.Instance.UpdateInventoryUI();
            Debug.Log("Lives: " + lives);
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        spawner.StopSpawning(); // Stop fruit spawning
        // Additional game over logic...
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart current scene
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your main menu scene name
    }

    void OnDestroy()
    {
        InventoryManager.Instance.OnInventoryChanged -= UpdatePowerupDisplay;
    }

    private void UpdatePowerupDisplay()
    {
        // Assuming you have Text elements in your UI for each powerup type
        if (UIManager.Instance.slowMotionQuantityText != null)
        {
            UIManager.Instance.slowMotionQuantityText.text = InventoryManager.Instance.GetPowerUpCount("SlowMotion").ToString();
        }
        if (UIManager.Instance.doublePointsQuantityText != null)
        {
            UIManager.Instance.doublePointsQuantityText.text = InventoryManager.Instance.GetPowerUpCount("DoublePoints").ToString();
        }
        if (UIManager.Instance.extraLifeQuantityText != null)
        {
            UIManager.Instance.extraLifeQuantityText.text = InventoryManager.Instance.GetPowerUpCount("ExtraLife").ToString();
        }
    }

}
