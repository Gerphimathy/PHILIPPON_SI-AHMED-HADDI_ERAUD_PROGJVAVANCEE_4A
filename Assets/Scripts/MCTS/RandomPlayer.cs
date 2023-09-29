using Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayer : APlayer
{
    public RandomPlayer(bool isP1) : base(isP1)
    {

    }
    public override Action GetAction(ref GameState gameState)
    {
        var possibles = gameState.GetPossibleActions(isP1);
        return (Action)UnityEngine.Random.Range(-1, 2);
    }
}
