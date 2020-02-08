using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonParrentScript : MonoBehaviour
{
    public void LoadStart()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
