using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _textMeshProUgui;
    
    void Start()
    { 
        _textMeshProUgui = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        int fps = (int)(1f / Time.unscaledDeltaTime);
        _textMeshProUgui.text = fps.ToString();
    }
}
