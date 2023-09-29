using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTimer : MonoBehaviour
{
    public GameManager gameManager;
    
    private TMPro.TextMeshProUGUI _textMeshProUgui;

    void Start()
    {
        _textMeshProUgui = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        UpdateTimer();
    }
    
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        var timer = gameManager.GameState.Timer;
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        string timerText;
        if(minutes>0)
            timerText = minutes + ":" + seconds.ToString("00");
        else
            timerText = seconds.ToString("00");
        _textMeshProUgui.text = timerText;
    }
}
