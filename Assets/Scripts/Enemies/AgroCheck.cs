using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroCheck : MonoBehaviour
{
    private Enemy enemy;
    private bool inRange;
    private Animator animator;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)"))
        {
            enemy.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            enemy.SelectMoveTarget();
            gameObject.SetActive(false);
            enemy.triggerArea.SetActive(true);
            enemy.inRange = false;
        }
    }
}
