using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public char[] letters;
    public bool[] guessedLetters;

    public Man man { get; private set; }

    public char[] word {  get; private set; }
    public char[] blanks { get; private set; }

    // Start is called before the first frame update
    public void Start()
    {
        // Initialize the game
        // Retrieve words into list
        // Load the level
        try
        {
            string dictionary = System.IO.File.ReadAllText("Assets/words.txt");
            words = dictionary.Split('\n');
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("I did not find words.txt!!!");
            words = new string[2] { "food", "drink" };
        }

        letters = new char[26] {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};

        guessedLetters = new bool[26] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};

        SceneManager.LoadScene("Level");
        NextLevel();
    }

    public void NextLevel()
    {
        this.level++;
        man.ResetMan();
        GetNewWord();
    }

    public void GetNewWord()
    {
        word = words[UnityEngine.Random.Range(0, words.Length)].ToCharArray();
        blanks = new char[word.Length];
        guessedLetters = new bool[26] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    }

    public LinkedList<int> guessLetter(int letter)
    {
        guessedLetters[letter] = true;
        // Check if the guessed letter is in the word
        char guess = letters[letter];
        LinkedList<int> indexes = new LinkedList<int>();
        // Get every instance of the letter in the word
        for(int i = 0; i< word.Length; i++)
            if(guess == word[i])
                indexes.AddLast(i);
        
        return indexes;
    }

    public void MakeAGuess(int letter)
    {
        LinkedList<int> indexes = guessLetter(letter);

        if (indexes.Count == 0)
            IncorrectGuess();
        else
            CorrectGuess(indexes.ToArrayPooled<int>());
    }

    public void CorrectGuess(int[] indexes)
    {
        // check if the level is complete
        foreach (int i in indexes)
            blanks[i] = word[i];

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

    }
    public void GameOver()
    {

    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(FindObjectOfType<Canvas>());
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.LoadScene("MainMenu");
    }
}
