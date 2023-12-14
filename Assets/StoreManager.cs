using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    //Credit system
    public int playerCredits;

    // Prices for the items can edit later 
    public int slowMotionPrice = 100;
    public int doublePointsPrice = 150;
    public int extraLifePrice = 200;
    public Text creditsText;

    public Text slowMotionQuantityText; 
    public Text doublePointsQuantityText; 
    public Text extraLifeQuantityText; 

    void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.SetInventoryUIText(slowMotionQuantityText, doublePointsQuantityText, extraLifeQuantityText);
        }

        InventoryManager.Instance.UpdateInventoryUI(); // This refreshes shop ui with the current inventory
    }


    public void BuyItem(string itemName)
    {
        int price = 0;

        switch (itemName)
        {
            case "SlowMotion":
                price = slowMotionPrice;
                break;
            case "DoublePoints":
                price = doublePointsPrice;
                break;
            case "ExtraLife":
                price = extraLifePrice;
                break;
        }

        if (playerCredits >= price)
        {
            playerCredits -= price;
            InventoryManager.Instance.AddPowerUp(itemName, 1);
            UpdateStoreUI();
        }
        else
        {
            Debug.Log("Not enough credits to purchase.");
            
        }
        InventoryManager.Instance.SaveInventory();
    }

    private void UpdateStoreUI()
    {
        if (creditsText != null)
        {
            creditsText.text = "Credits: " + Ninja_Player.Instance.credits.ToString();
        }
        InventoryManager.Instance.UpdateInventoryUI();
    }
    public void PurchaseSlowMotionPowerUp()
    {
        if (Ninja_Player.Instance.credits >= slowMotionPrice)
        {
            Ninja_Player.Instance.credits -= slowMotionPrice;
            InventoryManager.Instance.AddPowerUp("SlowMotion", 1);
            UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits);
            Ninja_Player.Instance.SaveCredits(); // This saves credits after buying
            InventoryManager.Instance.UpdateInventoryUI();
            InventoryManager.Instance.SaveInventory();
        }
        else
        {
            Debug.Log("Not enough credits.");
        }
    }




    public void DoublePointsPowerUp()
    {
        if (Ninja_Player.Instance.credits >= doublePointsPrice)
        {
            Ninja_Player.Instance.credits -= doublePointsPrice;
            InventoryManager.Instance.AddPowerUp("DoublePoints", 1);
            UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits); // Updates the UI
            Ninja_Player.Instance.SaveCredits();
            InventoryManager.Instance.UpdateInventoryUI();
            InventoryManager.Instance.SaveInventory();
        }
        else
        {
            Debug.Log("Not enough credits.");
        }
    }
    public void ExtraLifePowerUp()
    {
        if (Ninja_Player.Instance.credits >= extraLifePrice)
        {
            Ninja_Player.Instance.credits -= extraLifePrice;
            InventoryManager.Instance.AddPowerUp("ExtraLife", 1);
            UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits); // Updates the UI
            Ninja_Player.Instance.SaveCredits();
            InventoryManager.Instance.UpdateInventoryUI();
            InventoryManager.Instance.SaveInventory();
        }
        else
        {
            Debug.Log("Not enough credits.");
        }
    }
    public void PurchaseItem()
    {
        // After adjusting the players credits
        UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits);
    }

}
