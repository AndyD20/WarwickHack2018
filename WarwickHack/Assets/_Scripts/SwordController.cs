using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    private bool attackCooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !attackCooldown)
        {
            GameEnemy[] enemy = collision.transform.GetComponents<GameEnemy>();

            if (enemy.Length > 0)
            {
                enemy = collision.transform.GetComponentsInChildren<GameEnemy>();
            }

            if (enemy.Length > 0)
            {
                foreach (GameEnemy e in enemy)
                {
                    e.takeDamage();
                }
            }
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.2f);
        attackCooldown = true;
        yield return new WaitForSeconds(1);
        attackCooldown = false;
    }
}
