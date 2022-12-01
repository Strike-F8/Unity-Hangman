using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string[] words;

    public int score;
    public int level;
    public int wordLength;

    public Dictionary<char, bool> letters;

    public Man man { get; private set; }

    public char[] word {  get; private set; }
    public char[] blanks { get; private set; }
    
    public TextMeshProUGUI blanksDisplay;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI scoreDisplay;

    // Start is called before the first frame update

    public void StartGame()
    {
        // Initialize the game
        // Retrieve words into list
        // Load the level
        try
        {
            string dictionary = System.IO.File.ReadAllText("Assets/words.txt");
            words = dictionary.Split('\n');
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("I did not find words.txt!!!\nUsing default word list instead!!!!");
            words = new string[6] { "food", "drink", "Dessert", "Lunch", "Dinner", "Breakfast" };
        }

        letters = new Dictionary<char, bool>();
        for (char c = 'a'; c <= 'z'; c++)
            letters.Add(c, false);

        level = 1;
        score = 0;
        wordLength = 0;
        NextLevel();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level");
        this.level++;
        levelDisplay.text = $"Level {level}";
        man.ResetMan();
        GetNewWord();
        foreach (char c in blanks)
            blanksDisplay.text += "_ ";
    }

    public void GetNewWord()
    {
        // Get a new word from the list of words
        word = words[UnityEngine.Random.Range(0, words.Length)].ToCharArray();
        // Reset the display for the player's guesses
        blanks = new char[word.Length];
        // Reset the player's guesses
        for (char c = 'a'; c <= 'z'; c++)
            letters[c] = false;
    }

    public LinkedList<int> GuessLetter(char letter)
    {
        // Record that the player guessed this letter
        letters[letter] = true;
        // Check if the guessed letter is in the word
        LinkedList<int> indexes = new LinkedList<int>();
        // Get every instance of the letter in the word
        for (int i = 0; i < word.Length; i++)
            if (letter == word[i])
                indexes.AddLast(i);
        
        return indexes;
    }

    public void MakeAGuess(char letter)
    {
        LinkedList<int> indexes = GuessLetter(letter);

        if (indexes.Count == 0)
            IncorrectGuess();
        else
            CorrectGuess(indexes.ToArrayPooled<int>());
    }

    public void CorrectGuess(int[] indexes)
    {
        // Reveal the correctly guessed indexes
        foreach (int i in indexes)
            blanks[i] = word[i];
        
        // check if the level is complete
        if (blanks.ToString() == word.ToString())
            LevelComplete();
    }

    public void IncorrectGuess()
    {
        if (man.IncorrectGuess())
            GameOver();
    }

    public void LevelComplete()
    {
        // If this is the last level, load the game complete scene
        if (this.level >= 5)
            GameComplete();
        else
        // Load the win screen with a button to advance to the next level
            SceneManager.LoadScene("Win");
    }

    public void GameComplete()
    {
        SceneManager.LoadScene("GameComplete");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
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
        this.man = FindObjectOfType<Man>();
    }

    public void ClickLetter(string letter)
    {
        MakeAGuess((char)letter[0]);
    }
}
