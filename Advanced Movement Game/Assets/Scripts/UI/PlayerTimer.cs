using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    [HideInInspector] public float realTime;

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
            realTime += Time.deltaTime;
            UpdateText();
        }
    }

    void UpdateText()
    {
        float seconds = realTime % 60;
        float minutes = ((int)(realTime / 60) % 60);

        text.text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }
}
