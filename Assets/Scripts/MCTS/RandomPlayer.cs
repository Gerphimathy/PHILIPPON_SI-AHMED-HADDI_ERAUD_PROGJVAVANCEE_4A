using Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomPlayer : APlayer
{
    public RandomPlayer(bool isP1) : base(isP1)
    {

    }
    public override Action GetAction(ref GameState gameState)
    {
        return GetValidAction(ref gameState, isP1, Time.deltaTime);
    }
    public Action GetValidAction(ref GameState gameState, bool isP1,float deltaTime)
    {
        var possibles = gameState.GetPossibleActions(isP1, deltaTime).ToList();
        return possibles[UnityEngine.Random.Range(0, possibles.Count)];
    }
}
