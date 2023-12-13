using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs; // Array of fruit prefabs
    public float spawnInterval = 10.0f; // Interval between spawns, adjustable in the editor
    public float fadeDuration = 20.0f; // Duration for the fruit to completely fade away
    private Ninja_Player ninjaPlayer;
    private float timeLimit = 10f; // 60 seconds for Quickshot mode
    private float timeElapsed = 0f; // Time elapsed since the start
    private bool gameOverTriggered = false;
    void Start()
    {
        ninjaPlayer = FindObjectOfType<Ninja_Player>();
        // Start spawning fruits at intervals
        InvokeRepeating("SpawnFruit", 0, spawnInterval);
    }
    void Update()
    {
        if (timeElapsed < timeLimit)
        {
            timeElapsed += Time.deltaTime;
        }
        else if (!gameOverTriggered)
        {
            CancelInvoke("SpawnFruit");
            if (ninjaPlayer != null)
            {
                ninjaPlayer.GameOver();
                gameOverTriggered = true;
            }
        }
    }

    void SpawnFruit()
    {

        Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f)));
        GameObject fruit = Instantiate(fruitPrefabs[Random.Range(0, fruitPrefabs.Length)], new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);

        // Prevent fruits from falling
        Rigidbody2D rb2D = fruit.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            rb2D.isKinematic = true; // For 2D games
        }

        Rigidbody rb = fruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // For 3D games
        }

        // Start fade away process
        StartCoroutine(FadeAway(fruit));
    }

    IEnumerator FadeAway(GameObject fruit)
    {
        float currentTime = 0f;
        SpriteRenderer fruitRenderer = fruit.GetComponent<SpriteRenderer>();
        Color originalColor = fruitRenderer.color;

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            fruitRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(fruit);
    }
}