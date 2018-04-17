using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private Text timer;
    public int Seconds { get; set; }

	// Use this for initialization
	void Start () {
        timer = GetComponent<Text>();
        Seconds = 0;
        InvokeRepeating("UpdateTimer", 0, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateTimer()
    {
        Seconds += 1;
        timer.text = string.Format("Time: {0:00}:{1:00}", Seconds / 60, Seconds % 60);
    }
}
