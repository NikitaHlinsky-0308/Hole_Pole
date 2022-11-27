using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPScounter : MonoBehaviour
{
    public float updateInterval = 0.5f;
    [Range(10, 100)]
    public int fontSize = 24;
    private float _acuum = 0.0f;
    private int _frames = 0;
    private float _timeLeft;
    private float _fps;

    private GUIStyle _textStyle = new GUIStyle();
    
    
    void Start()
    {
        _timeLeft = updateInterval;

        _textStyle.fontStyle = FontStyle.Bold;
        _textStyle.fontSize = fontSize;
        _textStyle.normal.textColor = Color.black;
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _acuum += Time.timeScale / Time.deltaTime;
        ++_frames;

        if (_timeLeft <= 0.0f)
        {
            _fps = (_acuum / _frames);
            _timeLeft = updateInterval;
            _acuum = 0.0f;
            _frames = 0;
        }
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 200, 50), _fps.ToString("F4"), _textStyle);
    }
}
