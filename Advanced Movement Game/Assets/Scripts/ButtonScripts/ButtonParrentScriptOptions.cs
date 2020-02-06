using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonParrentScriptOptions : MonoBehaviour
{

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GetSliderValue(float value)
    {
        gameManager.newSensitivity = value;
    }
}
