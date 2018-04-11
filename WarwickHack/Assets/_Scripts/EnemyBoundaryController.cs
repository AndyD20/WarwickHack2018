using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoundaryController : MonoBehaviour {

    public Transform player;

    public void Start()
    {
        CapsuleCollider2D playerCollider = player.GetComponent<CapsuleCollider2D>();
        Debug.Log(playerCollider);

        foreach(Transform child in transform)
        {
            Physics2D.IgnoreCollision(child.GetComponent<BoxCollider2D>(), playerCollider, false);
        }
    }
}
