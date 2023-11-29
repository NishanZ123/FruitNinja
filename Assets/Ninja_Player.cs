using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja_Player : MonoBehaviour
{
    private Vector3 pos;
    public int score= 0; //Score
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        //If the game is running on an Android device
        if (Application.platform == RuntimePlatform.Android)
        {
        //If we are hitting the screen
            if (Input.touchCount == 1)
        {
            //Find screen touch position, by
            //transforming position from screen space into game world space.
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1));
            //Set position of the player object
            transform.position = new Vector3(pos.x,pos.y,3);
            //Set collider to true
            GetComponent<Collider2D>().enabled = true;
            return;
        }
            //Set collider to false
            GetComponent<Collider2D>().enabled = false;
        }
        else //If the game is not running on an iPhone device
        {
            //Find mouse position
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            //Set position
            transform.position = new Vector3(pos.x, pos.y, 3);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Fruit2D item = other.gameObject.GetComponent<Fruit2D>();

        if (other.tag == "Fruit")
        {
            if (item != null)
            {
                item.Hit();
                score++;
                Debug.Log(score);
            }
        }
        else if (other.tag == "Enemy")
        {
            if (item != null && item.isBomb)
            {
                item.Hit();
                score -= 2;
                Debug.Log(score);
            }
        }
    }
}