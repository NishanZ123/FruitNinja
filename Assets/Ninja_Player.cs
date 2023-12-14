using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ninja_Player : MonoBehaviour
{
    public AudioClip bombHitSound; //All sounds
    public AudioClip bassDropSound;
    public AudioClip slashingSound;
    public enum GameMode { Classic, Quickshot } //Difference between 2 gamemodes and tracks them
    public GameMode currentGameMode;
    public Spawn_items spawner;
    public GameObject gameOverPanel;
    public int lives = 3; // Starting number of lives
    public static Ninja_Player Instance { get; private set; }
    private Vector3 pos;
    public int score = 0; //Score
    public int credits; // credits
    public Text slowMotionQuantityText;
    public Text doublePointsQuantityText;
    public Text extraLifeQuantityText;
    public ParticleSystem bombExplosionEffect;

    
    void Start()
    {
        UIManager.Instance.InitializeNinjaGameUI(slowMotionQuantityText, doublePointsQuantityText, extraLifeQuantityText);//live update
        LoadCredits();
        UIManager.Instance.UpdateCreditsDisplay(credits);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Makes sure only one instance exists otherwise bugs appear
        }

            InventoryManager.Instance.OnInventoryChanged += UpdatePowerupDisplay;
            UpdatePowerupDisplay(); // Initial update 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // C for Credit mainly used for testing
        {
            AddCredits(1000); // Adds 1000 credits 
            SaveCredits(); // Save the new credits value
        }

        if (Input.GetKeyDown(KeyCode.R)) // R for Reset data which resets credit amount and powerup amount
        {
            ResetGameData();
        }

        if (Input.GetKeyDown(KeyCode.A)) //A for slowmode powerup
        {
            UseSlowDownTimePowerUp();
        }
        
        if (Input.GetKeyDown(KeyCode.S)) //S for extra life
        {
            UseExtraLifePowerUp();
        }

        if (Input.GetKeyDown(KeyCode.D)) //D for Double points
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
                AudioSource.PlayClipAtPoint(slashingSound, transform.position);
                // Check if double points power up is active and doubles it
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
                AudioSource.PlayClipAtPoint(bombHitSound, transform.position);

                // runs particle system
                if (bombExplosionEffect != null)
                {
                    bombExplosionEffect.transform.position = item.transform.position; // Position of particles on bomb
                    bombExplosionEffect.Play();
                }

                LoseLife(); // Lose a life when hitting a bomb
                Debug.Log("Lives: " + lives);
            }

        }
    }


    public void IncrementScore(int points)
    {
        score += points;
        
    }
    public void AddCredits(int amount)
    {
        credits += amount;
        UIManager.Instance.UpdateCreditsDisplay(credits);
        SaveCredits();
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
            float duration = 10f; // Duration of double points
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
            if (lives <= 0)
            {
                GameOver();
            }
        }
    }


    public void GainLife()
     {
        lives++;
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

    public void GameOver()
    {
        if (currentGameMode == GameMode.Quickshot)
        {
            // Handle game over for Quickshot mode
            gameOverPanel.SetActive(true);
            AudioSource.PlayClipAtPoint(bassDropSound, transform.position);
        }
        else
        {
            gameOverPanel.SetActive(true);
            spawner.StopSpawning(); // Stop fruit spawning
            AudioSource.PlayClipAtPoint(bassDropSound, transform.position);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart current scene
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    void OnDestroy()
    {
        InventoryManager.Instance.OnInventoryChanged -= UpdatePowerupDisplay;
    }

    private void UpdatePowerupDisplay()
    {
        
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
