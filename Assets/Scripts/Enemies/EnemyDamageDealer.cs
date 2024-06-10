using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()?.TakeDamage(damage);
        }
    }
}
