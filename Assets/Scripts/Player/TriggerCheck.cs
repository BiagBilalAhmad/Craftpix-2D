using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatform"))
        {
            Debug.Log($"Collided with {collision.gameObject.name}");
            player.transform.parent = collision.gameObject.transform;
            //player._rigidBody.gravityScale = 10f;
        }

        if (collision.CompareTag("Boundry"))
        {
            Invoke(nameof(ResetGame), 2f);
        }

        if (collision.CompareTag("Coin"))
        {
            GameManager.Instance.AddToCoins(1);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("LevelEnd"))
        {
            GameManager.Instance.NextLevel();
        }

        if (collision.CompareTag("Finish"))
        {
            GameManager.Instance.ShowVictory();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatform"))
        {
            player.transform.parent = null;
            //player._rigidBody.gravityScale = 1f;
        }
    }

    public void ResetGame()
    {
        GameManager.Instance.RestartGame();
    }
}
