using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float health = 5;
    public Transform heart;
    public Transform emptyHeart;
    public GameObject healthBar;


    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected bool attacking;

    // Use this for initialization
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> (); 
        animator = GetComponent<Animator> ();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (move.x > 0.01f)
        {
            if(spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        } 
        else if (move.x < -0.01f)
        {
            if(spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        if (Input.GetMouseButtonDown(0)) { attacking = true; velocity.x = 0; }
        else attacking = false;

        animator.SetBool("attacking", attacking);

        animator.SetBool("grounded", grounded);

        targetVelocity = move * maxSpeed;
    }

    private void UpdateHealth()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "platform")
        {
            transform.parent = null;
        }
    }
}