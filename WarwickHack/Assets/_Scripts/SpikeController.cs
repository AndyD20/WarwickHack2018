using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

    private bool isSpiking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isSpiking)
        {
            isSpiking = true;
            collision.GetComponent<PlayerPlatformerController>().Damaged = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isSpiking)
        {
            isSpiking = false;
        }
    }
}
