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

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.value = gameManager.newSensitivity;
        input = GameObject.Find("InputField").GetComponent<InputField>();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GetSliderValue(float value)
    {
        gameManager.newSensitivity = value;
    }

    public void GetInputValue(string text) {
        slider.value = int.Parse(text);
    }

    public void UpdateInputValue(float value)
    {
        input.text = value.ToString();
    }
}