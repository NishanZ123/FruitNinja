using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private float originalGravityScale = 1.0f; // Store the original gravity scale
    private bool isSlowMotionActive = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ActivateSlowMotion(float duration, float gravityScaleFactor)
    {
        if (isSlowMotionActive) return; // Prevent reactivation if already active

        originalGravityScale = Physics2D.gravity.y; // Store the current gravity
        Physics2D.gravity *= gravityScaleFactor; // Apply the slow motion effect

        isSlowMotionActive = true;
        Invoke("DeactivateSlowMotion", duration); // Schedule deactivation
    }

    private void DeactivateSlowMotion()
    {
        Physics2D.gravity = new Vector2(0, originalGravityScale); // Restore original gravity
        isSlowMotionActive = false;
    }
}
