using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public int maxHealth = 8;
    public Transform heart;
    public Transform emptyHeart;
    public GameObject healthBar;
    public Transform spawnPoint;
    public Text scoreText;
    public Transform sword;
    public DeathScreenController deathScreen;

    public bool Damaged { get; set; }
    public bool OutOfBounds { get; set; }
    public int CurrentHealth { get; set; }
    public int PlayerScore { get; set; }

    private bool lockMovement = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AnimationClip respawnClip;
    private AnimationClip hurtClip;
    private bool beingHurt = false;

    protected bool attacking;

    void Awake () 
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        spriteRenderer = GetComponent<SpriteRenderer> (); 
        animator = GetComponent<Animator> ();

        int healthModifier = PlayerPrefs.GetInt("difficulty", 1);

        maxHealth = (int)(maxHealth / healthModifier);
        CurrentHealth = maxHealth;
		SetHealth ();


        respawnClip = GetAnimationClip("Respawn");
        hurtClip = GetAnimationClip("Hurt");
        scoreText.text = "Score: 0000";

        sword.transform.GetComponent<BoxCollider2D>().enabled = false;
    }

	void SetHealth(){

        if (CurrentHealth > -1)
        {

            foreach (Transform child in healthBar.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            float pos = heart.GetComponent<RectTransform>().rect.width;
            float offset = pos;

            for (int i = 0; i < CurrentHealth; i++)
            {
                Transform newHeart = Instantiate(heart, healthBar.transform);

                if (i != 0)
                {
                    newHeart.transform.Translate(pos, 0, 0);
                    pos += offset;
                }
            }

            int difference = (int)(maxHealth - CurrentHealth);

            if (difference <= 0) return;

            for (int i = 0; i < difference; i++)
            {
                Transform newEmptyHeart = Instantiate(emptyHeart, healthBar.transform);

                if (CurrentHealth > 0)
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
	}

    protected override void FrameCalculations()
    {
        if (CurrentHealth > 0)
        {
            if (Damaged && !beingHurt)
            {
                beingHurt = true;

                CurrentHealth -= 1;
                Damaged = false;
                SetHealth();
                animator.SetTrigger("takingDamage");
                lockMovement = true;
                Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
                StartCoroutine(Hurt(rb));
            }

            if (OutOfBounds)
            {
                OutOfBounds = false;
                animator.SetTrigger("respawning");
                if (!beingHurt && !Damaged)
                {
                    CurrentHealth -= 1;
                    SetHealth();
                }
                lockMovement = true;
                transform.position = spawnPoint.transform.position;
                this.spriteRenderer.flipX = false;
                StartCoroutine(Respawn());
            }
        } else
        {
            animator.SetTrigger("dying");
            if (OutOfBounds) gameObject.SetActive(false);

            CurrentHealth -= 1;
            SetHealth();
            deathScreen.PlayerDeath();
        }
    }

    IEnumerator Hurt(Rigidbody2D rb)
    {        
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (!this.spriteRenderer.flipX)
        {
            rb.AddForce(-1 * transform.right * 10, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * 10, ForceMode2D.Impulse);
        }

        rb.AddForce(transform.up * 15, ForceMode2D.Impulse);

        yield return new WaitForSeconds(hurtClip.length);

        if (!grounded)
        {
            yield return new WaitForSeconds(1);
        }

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        lockMovement = false;

        yield return new WaitForSeconds(3);
        beingHurt = false;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnClip.length);
        lockMovement = false;        
    }

    public void RestoreLife(int amount)
    {
        StartCoroutine(RefillHearts(amount));
    }

    IEnumerator RefillHearts(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CurrentHealth += 1;
            SetHealth();
            yield return new WaitForSeconds(0.5f);
        }
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

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
            {
                animator.SetTrigger("attacking");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                StartCoroutine(SwordAttack());
            }
            

            animator.SetBool("grounded", grounded);


            targetVelocity = move * maxSpeed;
        }
    }

    IEnumerator SwordAttack()
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

        if (beingHurt && collision.gameObject.tag != "Background")
        {

            Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();

            if (rb)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.bodyType = RigidbodyType2D.Kinematic;
                lockMovement = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "platform")
        {
            transform.parent = null;
        }
    }

    public void ScorePoints(int points)
    {
        PlayerScore += points;
        scoreText.text = string.Format("Score: {0:0000}", PlayerScore);
    }

    public void WinLevel()
    {
        lockMovement = true;
        animator.SetTrigger("winning");
    }
}