using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    public float shootForce = 10f;
    public float damage = 20f;

    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * shootForce;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rot + 180f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 6 && !collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
