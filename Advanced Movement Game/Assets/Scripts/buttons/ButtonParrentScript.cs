using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonParrentScript : MonoBehaviour
{
    public void LoadStart()
    {
        SceneManager.LoadScene("Test Scene");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
