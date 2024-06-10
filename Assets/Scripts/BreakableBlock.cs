using System.Collections;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().enabled = true;
            StartCoroutine(BreakBlock());
        }
    }

    private IEnumerator BreakBlock()
    {
        yield return new WaitForSeconds(0.7f);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = false;
    }
}
