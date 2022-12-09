using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public int health { get; private set; }
    public Sprite[] states;

    public void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        ResetMan();
    }
    public bool IncorrectGuess()
    {
        health--;
        this.spriteRenderer.sprite = this.states[this.health - 1];
        return health <= 1;
    }

    public void ResetMan()
    {
        health = 7;
        this.spriteRenderer.sprite = this.states[this.health - 1];
    }
}
