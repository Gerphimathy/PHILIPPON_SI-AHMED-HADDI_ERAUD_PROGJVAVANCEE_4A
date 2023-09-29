using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = Pong.Action;

public class PseudoRandomPlayer : APlayer
{
    private float _remainingTime = 0;
    private float _timeBetweenActions = 0.5f;
    private float _timeBetweenActionsRandomRange = 0.2f;
    private Action _lastAction = Action.None;
    public PseudoRandomPlayer(bool isP1) : base(isP1)
    {

    }
    public override Action GetAction(GameState gameState)
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