using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePos : MonoBehaviour {

	void Update () {
        transform.position = Input.mousePosition;
	}
}
