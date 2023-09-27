using Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayer : IPlayer
{
    public Action GetAction()
    {
        return (Action)UnityEngine.Random.Range(-1, 2);
    }
}
