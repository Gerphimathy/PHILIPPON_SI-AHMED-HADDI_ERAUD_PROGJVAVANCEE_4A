using System;
using UnityEngine;

public struct Ball
{

    private Moveable _moveable;
    
    public Moveable Moveable => _moveable;

    public Vector3 Direction;
    

    private readonly Moveable _paddle1Moveable;
    public Moveable Paddle1Moveable => _paddle1Moveable;
    
    private readonly Moveable _paddle2Moveable;
    public Moveable Paddle2Moveable => _paddle2Moveable;
    
    


    public Ball(Moveable moveable, Vector3 direction, Moveable paddle1Moveable, Moveable paddle2Moveable)
    {
        _moveable = moveable;
        Direction = direction;
        this._paddle1Moveable = paddle1Moveable;
        this._paddle2Moveable = paddle2Moveable;
    }

    public void Move(ref Bounds terrainBounds, float delta)
    {
        Moveable.Move(Direction.normalized * delta);


    }
}