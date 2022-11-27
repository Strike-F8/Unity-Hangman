using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string[] words;

    public int score;
    public int level;

    //public Man man { get; private set; }

    //public Word word {  get; private set; }

    // Start is called before the first frame update
    public void Start()
    {
        // Initialize the game
        // Retrieve words into list
        // Load the first level
    }

    public void LoadLevel(int level)
    {
        this.level = level;
        SceneManager.LoadScene($"Level{level}");
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(FindObjectOfType<Canvas>());
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.LoadScene("MainMenu");
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.man = FindObjectOfType<Man>();
        //this.word = FindObjectOfType<Word>();
    }

}
