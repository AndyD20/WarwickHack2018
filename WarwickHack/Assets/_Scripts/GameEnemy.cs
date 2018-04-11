using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : MonoBehaviour {

    public int healthAmount = 1;
    public int scoreValue = 100;
    public Animator animator;
    public Transform parent_object;
    public PlayerPlatformerController player;

    private void Update()
    {
        if (healthAmount <= 0)
        {
            StartCoroutine(Die());
            healthAmount = 10000;
        }
    }

    IEnumerator Die()
    {
        animator.SetTrigger("dying");
        yield return new WaitForSeconds(GetAnimationClip("Dying").length);

        if (parent_object)
        {
            foreach (Transform child in parent_object)
            {
                Destroy(child.gameObject);
            }

            Destroy(parent_object.gameObject);
        }
        
        if (player) { player.scorePoints(scoreValue); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();
            player.damaged = true;
        }
    }

    public void takeDamage()
    {
        Debug.Log(string.Format("{0} - Taking damage", this.name));

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
