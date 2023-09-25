using System;
using UnityEngine;

public struct Ball
{

    private Moveable _moveable;
    
    public Moveable Moveable => _moveable;

    private Vector3 _direction;

    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value;
    }

    private readonly Moveable _paddle1Moveable;
    public Moveable Paddle1Moveable => _paddle1Moveable;
    
    private readonly Moveable _paddle2Moveable;
    public Moveable Paddle2Moveable => _paddle2Moveable;


    public Ball(Moveable moveable, Vector3 direction, Moveable paddle1Moveable, Moveable paddle2Moveable)
    {
        _moveable = moveable;
        _direction = direction;
        this._paddle1Moveable = paddle1Moveable;
        this._paddle2Moveable = paddle2Moveable;
    }

    public void Move(ref Bounds terrainBounds, float delta)
    {
     
        //TODO: Implement Ball Movement and Bounce 
        throw new NotImplementedException();
    }
}