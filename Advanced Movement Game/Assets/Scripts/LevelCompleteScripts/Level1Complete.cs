using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level1Complete : MonoBehaviour
{
    public OptionsSave optionsSave;
    public Text text;

    private GameManager gameManager;

    private void Start()
    {
        optionsSave = GameObject.Find("GameManager").GetComponent<OptionsSave>();
        text = GameObject.Find("TimerText").GetComponent<Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.level1CompleteTime = text.text;
        optionsSave.level1CompleteTime = text.text;
        optionsSave.StartSaveData();
        gameManager.Menu();
    }
}
