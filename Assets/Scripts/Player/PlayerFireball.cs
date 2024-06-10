using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    public float damage = 10f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        Vector2 v = rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 6 && !collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    public void Fire(bool right)
    {
        if (right)
        {
            transform.rotation = Quaternion.identity;
            rb.AddForce(new Vector2(speed, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            rb.AddForce(new Vector2(-speed, 0));
        }
    }
}
