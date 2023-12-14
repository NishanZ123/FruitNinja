using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text creditsText;
    public Text slowMotionQuantityText;
    public Text doublePointsQuantityText;
    public Text extraLifeQuantityText;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (Ninja_Player.Instance != null)
        {
            InitializeCreditsDisplay(); // Initialize credits display
        }
        else
        {
            Debug.LogWarning("Ninja_Player instance not found");
        }
    }

    public void UpdateCreditsDisplay(int credits)
    {
        creditsText.text = "Credits: " + credits;
    }

    private void InitializeCreditsDisplay()
    {
        if (creditsText != null)
        {
            creditsText.text = "Credits: " + Ninja_Player.Instance.credits;
        }
        else
        {
            Debug.LogWarning("creditsText not set in UIManager");
        }
    }

    public void InitializeNinjaGameUI(Text slowMotionText, Text doublePointsText, Text extraLifeText)
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
            slowMotionQuantityText.text = InventoryManager.Instance.GetPowerUpCount("SlowMotion").ToString();
        }
        if (doublePointsQuantityText != null)
        {
            doublePointsQuantityText.text = InventoryManager.Instance.GetPowerUpCount("DoublePoints").ToString();
        }
        if (extraLifeQuantityText != null)
        {
            extraLifeQuantityText.text = InventoryManager.Instance.GetPowerUpCount("ExtraLife").ToString();
        }
    }
}
