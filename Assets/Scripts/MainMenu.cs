using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartNewGame()
    {
        FindObjectOfType<GameManager>().StartGame();
    }
}
