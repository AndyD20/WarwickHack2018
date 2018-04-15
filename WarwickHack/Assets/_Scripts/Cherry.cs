using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cherry : MonoBehaviour {

    public int pointValue = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();
            player.ScorePoints(100);
            Destroy(this.gameObject);
        }
    }
}
