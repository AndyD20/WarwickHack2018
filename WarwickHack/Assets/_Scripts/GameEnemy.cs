using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : MonoBehaviour {

    public int healthAmount = 1;
    public int scoreValue = 100;
    public Animator animator;
    public Transform parent_object;
    public PlayerPlatformerController player;
    public SpriteRenderer enemySprite;
    public Rigidbody2D enemyRb;
    public bool isForTest = false;

    private bool isMoving = false;
    private bool isGrounded = true;
    private bool isDying = false;

    private void Start()
    {
        enemyRb.freezeRotation = true;
    }

    private void Update()
    {
        if (healthAmount <= 0)
        {
            StartCoroutine(Die());
            healthAmount = 10000;
        }

        if (enemyRb.velocity.y == 0)
        {
            isGrounded = true;
        }


        if (!isForTest)
        {
            int movementState = Random.Range(0, 99);

            if (movementState == 0 && !isMoving && isGrounded && !isDying)
            {
                enemySprite.flipX = false;
                StartCoroutine(Move(new Vector2(-5 + Random.Range(-2, 2), 5 + Random.Range(-1, 1))));
            }
            else if (movementState == 1 && !isMoving && isGrounded && !isDying)
            {
                enemySprite.flipX = true;
                StartCoroutine(Move(new Vector2(5 + Random.Range(-2, 2), 5 + Random.Range(-1, 1))));
            }
        }
    }
    
    void LateUpdate()
    {
        if (isDying) enemyRb.velocity = Vector2.zero;
    }

    IEnumerator Move(Vector2 velocity)
    {
        isMoving = true;
        animator.SetTrigger("jump");
        enemyRb.velocity = velocity;
        yield return new WaitForSeconds(GetAnimationClip("Jump").length + GetAnimationClip("Land").length);
        isMoving = false;
    }

    IEnumerator Die()
    {
        isDying = true;

        animator.SetTrigger("dying");
        yield return new WaitForSeconds(GetAnimationClip("Dying").length);

        if (parent_object)
        {
            foreach (Transform child in parent_object)
            {
                Destroy(child.gameObject);
            }

            Destroy(parent_object.gameObject);
        } else
        {
            Destroy(transform.gameObject);
        }
        
        if (player) { player.scorePoints(scoreValue); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDying)
        {
            PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();
            player.damaged = true;
        }
    }

    

    public void takeDamage()
    {
        healthAmount -= 1;
    }

    private AnimationClip GetAnimationClip(string name)
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
}
