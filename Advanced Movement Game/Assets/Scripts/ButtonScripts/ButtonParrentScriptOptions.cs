using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonParrentScriptOptions : MonoBehaviour
{

    private GameManager gameManager;
    private Slider slider;
    private InputField input;
    private OptionsSave optionsSave;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        optionsSave = gameManager.GetComponent<OptionsSave>();

        slider = GameObject.Find("Slider").GetComponent<Slider>();
        input = GameObject.Find("InputField").GetComponent<InputField>();
        slider.value = gameManager.mouseSensitivity;
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
        optionsSave.StartSaveData();
    }

    public void GetSliderValue(float value)
    {
        optionsSave.mouseSensitivity = value;
        gameManager.mouseSensitivity = value;
    }

    public void GetInputValue(string text) {
        slider.value = int.Parse(text);
    }

    public void UpdateInputValue(float value)
    {
        input.text = value.ToString();
    }
}