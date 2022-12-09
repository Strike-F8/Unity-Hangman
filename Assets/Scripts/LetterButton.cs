using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : Button
{

    private static readonly Color NeutralColor = Color.white;
    private static readonly Color CorrectColor = Color.green;
    private static readonly Color IncorrectColor = Color.red;

    public CanvasRenderer ButtonColor;

    public new void Awake()
    {
        ButtonColor = gameObject.GetComponent<CanvasRenderer>();
        ButtonColor.SetColor(NeutralColor);
    }
    public void OnClick(string letter)
    {
        ClickLetter(letter);
    }
    private void ClickLetter(string letter)
    {
        // Send the guessed letter to GameManager and change the color depending on the response
       bool response = FindObjectOfType<GameManager>().MakeAGuess((char)letter[0]);
       switch (response)
       {
            case true:
                ButtonColor.SetColor(CorrectColor);
                break;
            case false:
                ButtonColor.SetColor(IncorrectColor);
                break;
       }
       this.enabled = false; // don't allow the player to click this button anymore
    }
}
