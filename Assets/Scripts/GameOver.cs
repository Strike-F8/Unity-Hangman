using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    private string Word;
    public TextMeshProUGUI WordDisplay;

    public void Awake()
    {
        Word = FindObjectOfType<GameManager>().word.ArrayToString();
        WordDisplay.text = $"The word was {Word}";
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
