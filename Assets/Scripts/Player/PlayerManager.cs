using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] playableCharacters;

    public CameraFollow cam;

    void Start()
    {
        foreach (var character in playableCharacters)
        {
            character.SetActive(false);
        }

         GameObject player = playableCharacters[PlayerPrefs.GetInt("SelectedChar", 0)];
        player.SetActive(true);

        cam.target = player.transform;
    }
}
