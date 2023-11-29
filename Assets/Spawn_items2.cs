using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public float spawnTime = 2.0f; // Increase the spawn time for a slower rate
    public GameObject appleGravityPrefab;
    public float upForce = 5.0f; // Decrease the upward force for a slower ascent
    public float leftRightForce = 2.0f;
    public float maxX = -7.0f;
    public float minX = 7.0f;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);

        // Slightly lower the spawn position
        float spawnY = transform.position.y - 3f;

        // Instantiate the AppleGravity prefab with the adjusted position
        GameObject go = Instantiate(appleGravityPrefab, new Vector3(Random.Range(minX, maxX + 1), spawnY, 0f), Quaternion.identity) as GameObject;

        // Apply initial force to make it move upward and to the right.
        Vector3 force = new Vector3(leftRightForce, upForce, 0);
        go.transform.Translate(force * Time.deltaTime);

        StartCoroutine(Spawn());
    }
}
