using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour {

    public WinScreenController winScreen;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !activated)
        {
            activated = true;
            collision.GetComponent<PlayerPlatformerController>().WinLevel();
            winScreen.WinLevel();
        }
    }
}
