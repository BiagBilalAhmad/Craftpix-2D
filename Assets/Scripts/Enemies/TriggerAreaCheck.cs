using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemy.target = collision.transform;
            enemy.inRange = true;
            enemy.agroZone.SetActive(true);
        }
    }
}
