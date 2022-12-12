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

    [SerializeField] private AudioSource StartGameSound;

    public Canvas UI;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(UI.gameObject);
        UI.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
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

        level = 0;
        score = 0;
        wordLength = 0;
        UI.gameObject.SetActive(true);
        Destroy(FindObjectOfType<MainMenu>());
        NextLevel();
        StartGameSound.Play();
    }

    public void NextLevel()
    {
        foreach (var button in FindObjectsOfType<LetterButton>())
            button.ResetButton();

        UI.gameObject.SetActive(true);
        this.level++;
        levelDisplay.text = $"Level {level}";
        this.man = FindObjectOfType<Man>();
        man.ResetMan();
        GetNewWord();
    }

    public void GetNewWord()
    {
        // Get a new word from the list of words
        string temp = words[UnityEngine.Random.Range(0, words.Length)];
        word = temp[0..(temp.Length - 1)].ToCharArray();

        // Reset the display for the player's guesses
        blanks = new char[word.Length];
        wordLength = word.Length;
        // Reset the player's guesses
        for (int i = 0; i < blanks.Length; i++)
            blanks[i] = '_';
        UpdateBlanksDisplay();
        for (char c = 'a'; c <= 'z'; c++)
            letters[c] = false;
    }

    public void UpdateBlanksDisplay()
    {
        blanksDisplay.text = "";
        foreach (char c in blanks)
            blanksDisplay.text += c + " ";
    }

    public LinkedList<int> GuessLetter(char letter)
    {
        // Check if the guessed letter is in the word
        LinkedList<int> indexes = new();

        for (int i = 0; i < word.Length; i++)
            if (letter == word[i])
                indexes.AddLast(i);
        
        return indexes;
    }

    public bool MakeAGuess(char letter)
    {
        // Record that the player guessed this letter
        letters[letter] = true;
        // Get the indexes where the letter appears in the word
        LinkedList<int> indexes = GuessLetter(letter);

        if (indexes.Count == 0)
        {
            IncorrectGuess();
            return false;
        }
        else
            CorrectGuess(indexes.ToArrayPooled<int>());
        return true;
    }

    public void CorrectGuess(int[] indexes)
    {
        // Reveal the correctly guessed indexes
        foreach (int i in indexes)
            blanks[i] = word[i];
        UpdateBlanksDisplay();

        // check if the level is complete
        if (blanks.SequenceEqual(word))
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
        UI.gameObject.SetActive(false);
        SceneManager.LoadScene("GameComplete");
    }
    public void GameOver()
    {
        UI.gameObject.SetActive(false);
        SceneManager.LoadScene("GameOver");
    }

    public void ClickLetter(string letter)
    {
        MakeAGuess((char)letter[0]);
    }
}
