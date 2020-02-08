using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonParrentScripLevelSelect : MonoBehaviour
{
    private Text textLevel1;
    private GameManager gameManager;

    private void Start()
    {
        textLevel1 = GameObject.Find("Level1Time").GetComponent<Text>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        textLevel1.text = gameManager.level1CompleteTime;
    }

    public void LoadLevelOG()
    {
        SceneManager.LoadScene("TestLevel");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    } 

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
