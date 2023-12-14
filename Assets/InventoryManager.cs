using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Text slowMotionQuantityText;
    public Text doublePointsQuantityText;
    public Text extraLifeQuantityText;

    private Dictionary<string, int> powerUpInventory = new Dictionary<string, int>();

    void Start()
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
        LoadInventory(); // Load the inventory when the game starts
    }


    public void AddPowerUp(string powerUpName, int quantity)
    {
        if (!powerUpInventory.ContainsKey(powerUpName))
        {
            powerUpInventory[powerUpName] = 0;
        }
        powerUpInventory[powerUpName] += quantity;
        UpdateInventoryUI();
        NotifyInventoryChanged();
    }


    public bool UsePowerUp(string powerUpName)
    {
        if (powerUpInventory.ContainsKey(powerUpName) && powerUpInventory[powerUpName] > 0)
        {
            powerUpInventory[powerUpName]--;
            NotifyInventoryChanged(); // Notify after using a powerup
            return true; // Powerup was used successfully
        }
        return false; // Powerup wasn't used because it wasn't available
    }


    public int GetPowerUpCount(string powerUpName)
    {
        if (powerUpInventory.ContainsKey(powerUpName))
        {
            return powerUpInventory[powerUpName];
        }
        return 0;
    }

    public void SetInventoryUIText(Text slowMotionText, Text doublePointsText, Text extraLifeText)
    {
        slowMotionQuantityText = slowMotionText;
        doublePointsQuantityText = doublePointsText;
        extraLifeQuantityText = extraLifeText;
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
       
        if (slowMotionQuantityText != null)
        {
            slowMotionQuantityText.text = GetPowerUpCount("SlowMotion").ToString();
        }
        if (doublePointsQuantityText != null)
        {
            doublePointsQuantityText.text = GetPowerUpCount("DoublePoints").ToString();
        }
        if (extraLifeQuantityText != null)
        {
            extraLifeQuantityText.text = GetPowerUpCount("ExtraLife").ToString();
        }

        
    }
    public void SaveInventory()
    {
        PlayerPrefs.SetInt("SlowMotionCount", GetPowerUpCount("SlowMotion"));
        PlayerPrefs.SetInt("DoublePointsCount", GetPowerUpCount("DoublePoints"));
        PlayerPrefs.SetInt("ExtraLifeCount", GetPowerUpCount("ExtraLife"));
        PlayerPrefs.Save(); 
    }

    public void LoadInventory()
    {
            AddPowerUp("SlowMotion", PlayerPrefs.GetInt("SlowMotionCount", 0));
            AddPowerUp("DoublePoints", PlayerPrefs.GetInt("DoublePointsCount", 0));
            AddPowerUp("ExtraLife", PlayerPrefs.GetInt("ExtraLifeCount", 0));
        UpdateInventoryUI(); 
    }
    public void ResetInventory()
    {
       
        powerUpInventory["SlowMotion"] = 0;
        powerUpInventory["DoublePoints"] = 0;
        powerUpInventory["ExtraLife"] = 0;

        
        UpdateInventoryUI();
    }

    public void RemovePowerUp(string powerUpName, int quantity)
    {
        if (powerUpInventory.ContainsKey(powerUpName))
        {
            powerUpInventory[powerUpName] = Mathf.Max(0, powerUpInventory[powerUpName] - quantity);
            UpdateInventoryUI();

            // Debug log to display the count after updating
            Debug.Log(powerUpName + " Power-ups remaining: " + powerUpInventory[powerUpName]);
            NotifyInventoryChanged();
        }
    }


    void OnApplicationQuit()
    {
        SaveInventory();
    }

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;
    
    private void NotifyInventoryChanged()
    {
        if (OnInventoryChanged != null)
        {
            OnInventoryChanged();
        }
    }
}

