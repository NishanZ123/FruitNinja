using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ninja_Player : MonoBehaviour
{
    public static Ninja_Player Instance { get; private set; }
    private Vector3 pos;
    public int score = 0; //Score
    public int credits; // credits
    // Start is called before the first frame update
    void Start()
    {
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
                score++;
                credits++;
                Debug.Log(score);
                Debug.Log(credits);
            }
        }
        else if (other.tag == "Enemy")
        {
            if (item != null && item.isBomb)
            {
                item.Hit();
                score -= 2;
                Debug.Log(score);
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

}