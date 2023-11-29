using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleFollow : MonoBehaviour
{
    public Vector3 tf;
    public Vector3 fd;
    public float moveSpeed = 5.0f; // Adjust the movement speed

    void Start()
    {
        tf = this.transform.up;
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the vector from the object to the mouse cursor
        fd = mousePosition - this.transform.position;
        float angle = Vector2.Angle(new Vector2(tf.x, tf.y), new Vector2(fd.x, fd.y));

        Debug.DrawRay(this.transform.position, tf * 2, Color.green);
        Debug.DrawRay(this.transform.position, fd, Color.red);

        Vector3 crossP = Vector3.Cross(tf, fd);
        int clockwise = 1;
        if (crossP.z < 0)
        {
            clockwise = -1;
        }

        this.transform.rotation = Quaternion.Euler(0, 0, angle * clockwise);

        // Move the object towards the mouse cursor
        this.transform.Translate(this.transform.up * moveSpeed * Time.deltaTime);
    }
}
