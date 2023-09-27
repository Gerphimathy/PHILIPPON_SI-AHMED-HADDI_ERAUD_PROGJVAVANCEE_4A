using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = Pong.Action;

public class PseudoRandomPlayer : IPlayer
{
    private float _remainingTime = 0;
    private float _timeBetweenActions = 3f;
    private float _timeBetweenActionsRandomRange = 0.5f;
    private Action _lastAction = Action.None;
    
    public Action GetAction()
    {
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0)
        {
            _lastAction = (Action)UnityEngine.Random.Range(-1, 2);
            _remainingTime = _timeBetweenActions + UnityEngine.Random.Range(-_timeBetweenActionsRandomRange, _timeBetweenActionsRandomRange);
        }
        return _lastAction;
    }
}