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
    private PlayerTimer playerTimer;

    private void Start()
    {
        optionsSave = GameObject.Find("GameManager").GetComponent<OptionsSave>();
        text = GameObject.Find("TimerText").GetComponent<Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerTimer = GameObject.Find("TimerText").GetComponent<PlayerTimer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.realLevel1CompleteTime > playerTimer.realTime || gameManager.realLevel1CompleteTime == 0f) 
        { 
            gameManager.level1CompleteTime = text.text;
            gameManager.realLevel1CompleteTime = playerTimer.realTime;
            optionsSave.level1CompleteTime = text.text;
            optionsSave.realLevel1CompleteTime = playerTimer.realTime;
            optionsSave.StartSaveData();
        }
        gameManager.Menu();
    }
}
