using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public Transform spawnPoint;

    private bool active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !active)
        {
            spawnPoint.position = transform.position;
        }
    }
}
