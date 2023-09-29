using System;
using UnityEngine;
using Action = Pong.Action;


public class Player : APlayer
{
    public Player(bool isP1) : base(isP1)
    {
        SetScheme(isP1 ? Scheme.ZQSD : Scheme.Arrow);
    }
    public enum Scheme
    {
        Arrow,
        ZQSD
    }
    
    
    private KeyCode _up;
    private KeyCode _down;
    
    public void SetScheme(Scheme scheme)
    {
        switch (scheme)
        {
            case Scheme.Arrow:
                _up = KeyCode.UpArrow;
                _down = KeyCode.DownArrow;
                break;
            case Scheme.ZQSD:
                _up = KeyCode.W;
                _down = KeyCode.S;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null);
        }
    }
    
    public override Action GetAction(GameState gameState)
    {
        if (Input.GetKey(_up))
            return Action.Up;
        if (Input.GetKey(_down))
            return Action.Down;
        return Action.None;
    }
}