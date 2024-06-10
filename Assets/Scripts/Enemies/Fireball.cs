using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 10;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(-Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
