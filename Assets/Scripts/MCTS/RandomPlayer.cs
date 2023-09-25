using Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayer : IPlayer
{
    public Action GetAction()
    {
        throw new System.NotImplementedException("Waiting for possible moves");
    }
}
