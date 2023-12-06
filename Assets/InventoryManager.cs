using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Text slowMotionQuantityText;
    public Text doublePointsQuantityText;
    public Text extraLifeQuantityText;

    private Dictionary<string, int> powerUpInventory = new Dictionary<string, int>();

    void Start()
    {
        LoadInventory();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    public void AddPowerUp(string powerUpName, int quantity)
    {
        if (!powerUpInventory.ContainsKey(powerUpName))
        {
            powerUpInventory[powerUpName] = 0;
        }
        powerUpInventory[powerUpName] += quantity;
        UpdateInventoryUI();
    }


    public bool UsePowerUp(string powerUpName)
    {
        if (powerUpInventory.ContainsKey(powerUpName) && powerUpInventory[powerUpName] > 0)
        {
            powerUpInventory[powerUpName]--;
            return true;
        }
        return false;
    }

    public int GetPowerUpCount(string powerUpName)
    {
        if (powerUpInventory.ContainsKey(powerUpName))
        {
            return powerUpInventory[powerUpName];
        }
        return 0;
    }

    public void UpdateInventoryUI()
    {
        // Update the quantity display for each power-up in the inventory
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

        // Update other inventory UI elements as needed
    }
    public void SaveInventory()
    {
        PlayerPrefs.SetInt("SlowMotionCount", GetPowerUpCount("SlowMotion"));
        PlayerPrefs.SetInt("DoublePointsCount", GetPowerUpCount("DoublePoints"));
        PlayerPrefs.SetInt("ExtraLifeCount", GetPowerUpCount("ExtraLife"));
        PlayerPrefs.Save(); // Don't forget to call this to write to disk
    }

    public void LoadInventory()
    {
            AddPowerUp("SlowMotion", PlayerPrefs.GetInt("SlowMotionCount", 0));
            AddPowerUp("DoublePoints", PlayerPrefs.GetInt("DoublePointsCount", 0));
            AddPowerUp("ExtraLife", PlayerPrefs.GetInt("ExtraLifeCount", 0));
        UpdateInventoryUI(); // Make sure to update the UI with the loaded values
    }
    public void ResetInventory()
    {
        // Code to reset all power-up counts to 0
        // Example:
        powerUpInventory["SlowMotion"] = 0;
        powerUpInventory["DoublePoints"] = 0;
        powerUpInventory["ExtraLife"] = 0;

        // Don't forget to update the UI if necessary
        UpdateInventoryUI();
    }

}

