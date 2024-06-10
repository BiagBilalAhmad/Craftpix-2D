using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject selectionScreen;

    public void StartGame()
    {
        PlayerPrefs.SetInt("Coins", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {

    }

    public void OpenSelectionScreen()
    {
        mainMenuScreen.SetActive(false);
        selectionScreen.SetActive(true);
    }

    public void SelectCharacter(int charNum)
    {
        PlayerPrefs.SetInt("SelectedChar", charNum);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
