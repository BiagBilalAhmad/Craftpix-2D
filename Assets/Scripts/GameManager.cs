using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Coins")]
    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    [Header("Fireball")]
    public GameObject fireballPrefab;
    public Transform[] shootPoints;

    private void Start()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);

        AddToCoins(PlayerPrefs.GetInt("Coins", 0));

        if(SceneManager.GetActiveScene().name != "Level 3")
            StartCoroutine(ShootFireBalls());
    }

    public IEnumerator ShootFireBalls()
    {
        while (true)
        {
            float delay = Random.Range(4f, 8f);
            yield return new WaitForSeconds(delay);
            var shootPoint = shootPoints[Random.Range(0, shootPoints.Length)];

            Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
        }
    }

    public void AddToCoins(int coinAmount)
    {
        coinCount += coinAmount;
        PlayerPrefs.SetInt("Coins", coinCount);
        coinText.text = coinCount.ToString();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void ShowVictory()
    {
        Time.timeScale = 0f;
        victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
