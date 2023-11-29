using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customGravity : MonoBehaviour
{
    public Vector2 acceleration;
    public Vector2 gravity;
    public Vector2 push;

    private Vector2 velocity;
    private float mass = 10.0f;

    private void ApplyForce(Vector2 force)
    {
        Vector2 a = force / mass;
        acceleration += a;
    }

    private void UpdatePos()
    {
        velocity = velocity + acceleration;
        transform.position += (Vector3)(velocity * Time.deltaTime);
        acceleration = new Vector2(0.0f, 0.0f); // reset to zero
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = new Vector2(0, -1);
        push = new Vector2(100, 200); // upward left
        ApplyForce(push); // apply only once
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ApplyForce(gravity); // apply continuously
        UpdatePos();
    }
}
