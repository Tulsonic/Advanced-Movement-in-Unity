using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManagerInstance;

    private OptionsSave optionsSave;

    [HideInInspector] public float mouseSensitivity;

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (gameManagerInstance != this)
        {
            Object.Destroy(gameObject);
        }

        //SceneManager.LoadScene("TestLevel");
    }

    private void Start()
    {
        optionsSave = GetComponent<OptionsSave>();

        // Get Defaults
        string path = Application.persistentDataPath + "/player.AMGD";
        if (File.Exists(path)) { GetSavedValues(); }
        else 
        {
            mouseSensitivity = optionsSave.mouseSensitivity; 
        }
    }

    public void GetSavedValues()
    {
        SettingsData data = SaveSystem.LoadData();
        
        // PlayerCamera
        mouseSensitivity = data.mouseSensitivity;
    }

    private void Update()
    {
        if (GameObject.Find("PlayerCamera") != null) 
        {
            GameObject.Find("PlayerCamera").GetComponent<PlayerCameraController>().mouseSensitivity = mouseSensitivity; 
        }

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
