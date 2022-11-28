using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public int health { get; private set; }
    public Sprite[] states { get; private set; }

    public void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool IncorrectGuess()
    {
        health--;
        if (health <= 0)
            return true;
        else
        {
            return false;
        }
    }

    public void ResetMan()
    {
        health = 6;
        this.spriteRenderer.sprite = this.states[this.health - 1];
    }
}
