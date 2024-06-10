using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
}
