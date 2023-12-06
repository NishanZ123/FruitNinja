using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    // Assuming you have a currency system in place
    public int playerCredits;

    // Prices for the items
    public int slowMotionPrice = 100;
    public int doublePointsPrice = 150;
    public int extraLifePrice = 200;
    public Text creditsText;


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
            // Optionally, update the UI to reflect that the player cannot afford the item
        }
    }

    private void UpdateStoreUI()
    {
        if (creditsText != null)
        {
            creditsText.text = "Credits: " + Ninja_Player.Instance.credits.ToString();
        }
    }

    public void PurchaseSlowMotionPowerUp()
    {
        if (Ninja_Player.Instance.credits >= slowMotionPrice)
        {
            Ninja_Player.Instance.credits -= slowMotionPrice;
            InventoryManager.Instance.AddPowerUp("SlowMotion", 1);
            UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits);
            Ninja_Player.Instance.SaveCredits(); // Save credits after purchase
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
            UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits); // Update the UI
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
            UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits); // Update the UI
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
        // After adjusting the player's credits
        UIManager.Instance.UpdateCreditsDisplay(Ninja_Player.Instance.credits);
    }

}
