using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour {

    public bool moveY = false;
    public float moveDistance;
    public float speed;
    public float delay = 0;

    private float limLow;
    private float limHigh;
    private bool moveDownLeft;

	// Use this for initialization
	void Start () {

        if (moveY) { limLow = transform.position.y - moveDistance; limHigh = transform.position.y + moveDistance; }
        else { limLow = transform.position.x - moveDistance; limHigh = transform.position.x + moveDistance; }

        moveDownLeft = true;

    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(movePlatform());
    }

    IEnumerator movePlatform()
    {
        yield return new WaitForSeconds(delay);

        if (moveY && moveDownLeft)
        {
            if (transform.position.y - speed >= limLow)
            {
                transform.Translate(0, speed * -1, 0);
            }
            else
            {
                moveDownLeft = false;
            }
        }

        else if (moveY && !moveDownLeft)
        {
            if (transform.position.y + speed <= limHigh)
            {
                transform.Translate(0, speed, 0);
            }
            else
            {
                moveDownLeft = true;
            }
        }

        if (!moveY && moveDownLeft)
        {

            if (transform.position.x - speed >= limLow)
            {
                transform.Translate(speed * -1, 0, 0);
            }
            else
            {
                moveDownLeft = false;
            }

        }

        else if (!moveY && !moveDownLeft)
        {
            if (transform.position.x + speed <= limHigh)
            {
                transform.Translate(speed, 0, 0);
            }
            else
            {
                moveDownLeft = true;
            }
        }
    }
}
