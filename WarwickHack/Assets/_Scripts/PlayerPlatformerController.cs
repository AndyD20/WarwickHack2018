using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float maxHealth = 5;
    public Transform heart;
    public Transform emptyHeart;
    public GameObject healthBar;
    public bool damaged { get; set; }
    public bool outOfBounds { get; set; }
    public Transform spawnPoint;
    public Text scoreText;
    public Transform sword;

    private int playerScore;
    private bool lockMovement = false;
    private float currentHealth = 0;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AnimationClip respawnClip;
    private AnimationClip hurtClip;

    protected bool attacking;

    // Use this for initialization
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> (); 
        animator = GetComponent<Animator> ();
        currentHealth = maxHealth;
		setHealth ();


        respawnClip = GetAnimationClip("Respawn");
        hurtClip = GetAnimationClip("Hurt");
        scoreText.text = "Score: 0000";

        sword.transform.GetComponent<BoxCollider2D>().enabled = false;
    }

	void setHealth(){

        foreach(Transform child in healthBar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float pos = heart.GetComponent<RectTransform>().rect.width;
        float offset = pos;

		for (int i = 0; i < currentHealth; i++) {
            Transform newHeart = Instantiate(heart, healthBar.transform);

            if (i != 0)
            {
                newHeart.transform.Translate(pos, 0, 0);
                pos += offset;
            }
        }

        int difference = (int)(maxHealth - currentHealth);

        if (difference <= 0) return;

        for (int i = 0; i < difference; i++)
        {
            Transform newEmptyHeart = Instantiate(emptyHeart, healthBar.transform);

            if (currentHealth > 0)
            {
                newEmptyHeart.transform.Translate(pos, 0, 0);
                pos += offset;
            }
            else if (i != 0)
            {
                newEmptyHeart.transform.Translate(pos, 0, 0);
                pos += offset;
            }

        }
	}

    protected override void FrameCalculations()
    {

        if (damaged)
        {
            currentHealth -= 1;
            damaged = false;
            setHealth();
            animator.SetTrigger("takingDamage");
            lockMovement = true;
            Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
            StartCoroutine(hurt(rb));
        }

        if (outOfBounds)
        {
            animator.SetTrigger("respawning");
            currentHealth -= 1;
            setHealth();
            lockMovement = true;
            transform.position = spawnPoint.transform.position;
            this.spriteRenderer.flipX = false;
            StartCoroutine(respawn());
            outOfBounds = false;
        }
    }

    // TODO Fix the issue where the velocity drops off after the hurting animation ends.
    IEnumerator hurt(Rigidbody2D rb)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (!this.spriteRenderer.flipX)
        {
            rb.AddForce(-1 * transform.right * 10, ForceMode2D.Impulse);
        } else
        {
            rb.AddForce(transform.right * 10, ForceMode2D.Impulse);
        }

        rb.AddForce(transform.up * 15, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(hurtClip.length);
        lockMovement = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(respawnClip.length);
        lockMovement = false;
    }

    public AnimationClip GetAnimationClip(string name)
    {
        if (!animator) return null; // no animator

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null; // no clip by that name
    }

    protected override void ComputeVelocity()
    {
        if (!lockMovement)
        {
            Vector2 move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
                animator.SetTrigger("jump");

            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            if (move.x > 0.01f)
            {
                if (spriteRenderer.flipX == true)
                {
                    spriteRenderer.flipX = false;
                    sword.GetComponent<BoxCollider2D>().offset = new Vector2(0.8f, 0.2f);
                }
            }
            else if (move.x < -0.01f)
            {
                if (spriteRenderer.flipX == false)
                {
                    spriteRenderer.flipX = true;
                    sword.GetComponent<BoxCollider2D>().offset = new Vector2(-3.5f, 0.2f);
                }
            }


            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("attacking");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                StartCoroutine(swordAttack());
            }
            

            animator.SetBool("grounded", grounded);


            targetVelocity = move * maxSpeed;
        }
    }

    IEnumerator swordAttack()
    {
        yield return new WaitForSeconds(0.2f);
        sword.transform.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(GetAnimationClip("Attacking").length);
        sword.transform.GetComponent<BoxCollider2D>().enabled = false;
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

    public void scorePoints(int points)
    {
        playerScore += points;
        scoreText.text = string.Format("Score: {0:0000}", playerScore);
    }
}