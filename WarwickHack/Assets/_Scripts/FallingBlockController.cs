using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingBlockController : MonoBehaviour {

    private bool isFalling = false;
    private Vector2 originalPosition;
    private Quaternion originalRotation;

    public float fallTime = 1;


    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isFalling)
        {
            StartCoroutine(BlockFall());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && !isFalling)
        {
            StartCoroutine(BlockFall());
        }
    }

    IEnumerator BlockFall()
    {
        isFalling = true;
        transform.DOMoveX(transform.position.x - 0.08f, fallTime / 10).SetLoops(Mathf.FloorToInt(fallTime * 10), LoopType.Yoyo);
        yield return new WaitForSeconds(fallTime);
        gameObject.AddComponent<Rigidbody2D>();
        yield return new WaitForSeconds(5);
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isFalling = false;
    }
}
