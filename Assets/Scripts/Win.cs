using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void NextLevel()
    {
        FindObjectOfType<GameManager>().NextLevel();
        Destroy(this.gameObject);
    }
}
