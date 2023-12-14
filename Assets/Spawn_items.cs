using System.Collections;
using UnityEngine;

public class Spawn_items : MonoBehaviour
{
    private Coroutine spawnCoroutine;
    public float spawnTime = 1; // Spawn Time
    public GameObject[] fruits; // Array of fruit prefabs
    public GameObject bomb; // Bomb prefab
    public float upForce = 750; // Up force, adjustable in the editor
    public float enhancedUpForce = 500; // Enhanced up force for slow-motion mode
    public float leftRightForce = 200; // Left and right force
    public float maxX = -7; // Max x spawn position
    public float minX = 7; // Min x spawn position

    void Start()
    {
        spawnCoroutine = StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            GameObject prefab = Random.Range(0, 100) < 30 ? bomb : fruits[Random.Range(0, fruits.Length)];
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(minX, maxX), transform.position.y, 0);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            GameObject go = Instantiate(prefab, spawnPosition, randomRotation);
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float currentUpForce = upForce;
                if (PowerUpManager.Instance != null && PowerUpManager.Instance.IsSlowMotionActive)
                {
                    currentUpForce = enhancedUpForce; // Use enhanced force during slow-motion
                }

                float horizontalForce = go.transform.position.x > 0 ? -leftRightForce : leftRightForce;

                Vector2 force = new Vector2(horizontalForce, currentUpForce);
                rb.AddForce(force);

                rb.angularVelocity = Random.Range(-100f, 100f);
            }
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

}
