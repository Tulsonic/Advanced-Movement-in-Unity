using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    private float timer;
    private bool isActive;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        isActive = true;
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            UpdateText();
        }
    }

    void UpdateText()
    {
        float seconds = timer % 60;
        float minutes = ((int)(timer / 60) % 60);

        text.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }
}
