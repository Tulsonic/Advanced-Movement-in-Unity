using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManagerInstance;

    [HideInInspector] public float newSensitivity;

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
        newSensitivity = 100f;
    }

    private void Update()
    {
        Debug.Log(newSensitivity);
        if (GameObject.Find("PlayerCamera") != null) 
        {
            GameObject.Find("PlayerCamera").GetComponent<PlayerCameraController>().mouseSensitivity = newSensitivity; 
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
