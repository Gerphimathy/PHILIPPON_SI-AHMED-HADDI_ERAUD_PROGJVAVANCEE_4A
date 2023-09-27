using System;
using UnityEngine;
using Action = Pong.Action;


public class Player : IPlayer
{
    public enum Scheme
    {
        Arrow,
        ZQSD
    }
    
    
    private KeyCode _up;
    private KeyCode _down;
    
    public Player(Scheme scheme)
    {
        switch (scheme)
        {
            case Scheme.Arrow:
                _up = KeyCode.UpArrow;
                _down = KeyCode.DownArrow;
                break;
            case Scheme.ZQSD:
                _up = KeyCode.Z;
                _down = KeyCode.S;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null);
        }
    }
    
    public Action GetAction()
    {
        if (Input.GetKey(_up))
            return Action.Up;
        if (Input.GetKey(_down))
            return Action.Down;
        return Action.None;
    }
}