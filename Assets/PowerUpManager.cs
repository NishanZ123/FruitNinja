using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public bool IsSlowMotionActive => isSlowMotionActive;
    public float CurrentVelocityFactor => velocityFactor;
    private bool isDoublePointsActive = false;
    public bool IsDoublePointsActive => isDoublePointsActive;


    [SerializeField] private float slowDownDuration = 5f;
    [SerializeField] private float velocityFactor = 0.5f;
    [SerializeField] private float gravityScaleFactor = 0.5f;

    private List<Rigidbody2D> affectedRigidbodies = new List<Rigidbody2D>();
    private Dictionary<Rigidbody2D, Vector2> originalVelocities = new Dictionary<Rigidbody2D, Vector2>();
    private float originalGravity; // Declare originalGravity at class scope
    private bool isSlowMotionActive = false;

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
    }

    public void ActivateSlowMotion()
    {
        affectedRigidbodies.Clear();
        if (isSlowMotionActive) return;

        originalVelocities.Clear();

        // Store original gravity scale
        originalGravity = Physics2D.gravity.y;
        Physics2D.gravity = new Vector2(Physics2D.gravity.x, originalGravity * gravityScaleFactor);

        // Find and slow down all relevant objects
        foreach (var rb in FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.CompareTag("Fruit") || rb.CompareTag("Enemy"))
            {
                affectedRigidbodies.Add(rb);
                originalVelocities[rb] = rb.velocity;
                rb.velocity *= velocityFactor;
            }
        }

        isSlowMotionActive = true;
        Invoke(nameof(DeactivateSlowMotion), slowDownDuration);
    }

    private void DeactivateSlowMotion()
    {
        // Restore original gravity and velocities
        Physics2D.gravity = new Vector2(Physics2D.gravity.x, originalGravity);

        foreach (var rb in affectedRigidbodies)
        {
            if (rb != null && originalVelocities.ContainsKey(rb))
            {
                rb.velocity = originalVelocities[rb];
            }
        }

        isSlowMotionActive = false;
    }

    public void ActivateDoublePoints(float duration)
    {
        if (isDoublePointsActive) return;

        isDoublePointsActive = true;
        Invoke(nameof(DeactivateDoublePoints), duration);
    }
    
    private void DeactivateDoublePoints()
    {
        isDoublePointsActive = false;
    }
}
