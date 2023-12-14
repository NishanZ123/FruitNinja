using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit2D : MonoBehaviour
{
    private bool canBeDead = false; 
    private Vector3 screen; 
    private Ninja_Player playerScript;

    public GameObject splatPrefab;
    public bool isBomb = false;

    
    public int score = 0; // Score

    void Start()
    {
        canBeDead = false; 
        playerScript = FindObjectOfType<Ninja_Player>();
    }

    void Update()
    {
        // Set screen position
        screen = Camera.main.WorldToScreenPoint(transform.position);

        // If we can die and are not on the screen
        if (canBeDead && screen.y < -20)
        {
            // Destroy
            Destroy(gameObject);
        }
        else if (!canBeDead && screen.y > -10)
        {
            // We can die
            canBeDead = true;
        }
        // ...
    }

    public void Hit()
    {
        if (!isBomb) //Only spawn the splat if it's a fruit
        {
            Instantiate(splatPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);

    }
}
