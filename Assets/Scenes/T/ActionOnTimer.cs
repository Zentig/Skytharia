using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ActionOnTimer : MonoBehaviour
{
    private Action _timerCallback;
    private float _timer;
    bool _isStarted;

    public void SetTimer(float time, Action timerCallback)
    {
        _timer = time;
        _timerCallback = timerCallback;
        _isStarted = true;
    }
    private void Update()
    {
        if (_isStarted)
        {
            if (_timer < 0f)
            {
                _timer -= Time.deltaTime;
            }

            if (IsTimerComplete())
            {
                _timerCallback();
                _isStarted = false;
            }
        } 
    }
    public bool IsTimerComplete()
    {
        return _timer <= 0f;
    }
}
