using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private Text timer;
    private int seconds;

	// Use this for initialization
	void Start () {
        timer = GetComponent<Text>();
        seconds = 0;
        InvokeRepeating("UpdateTimer", 0, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateTimer()
    {
        seconds += 1;
        timer.text = string.Format("Time: {0:00}:{1:00}", seconds / 60, seconds % 60);
    }
}
