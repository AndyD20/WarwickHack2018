﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    private bool attackCooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy" && !attackCooldown)
        {
            GameEnemy enemy = collision.transform.GetComponent<GameEnemy>();

            if (enemy)
            {
                enemy.takeDamage();
            }

            StartCoroutine(Cooldown());

            Debug.Log("Attack succeed");
        }
    }

    IEnumerator Cooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(1);
        attackCooldown = false;
    }
}