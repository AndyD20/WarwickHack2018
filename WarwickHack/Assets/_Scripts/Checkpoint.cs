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
            active = true;
            spawnPoint.position = transform.position;


            PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();
            player.RestoreLife(player.maxHealth - player.CurrentHealth);
        }
    }
}
