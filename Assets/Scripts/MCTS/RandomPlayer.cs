using Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayer : IPlayer
{
    public Action GetAction()
    {
        switch (Time.time % 3)
        {
            case 0: return Action.Up;
            case 1: return Action.Down;
            case 2: default: return Action.None;
        }
    }
}
