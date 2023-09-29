using System;
using UnityEngine;

public struct Ball
{

    public Moveable Moveable;
    
    public Vector3 Direction;
    

    private readonly Moveable _paddle1Moveable;
    public Moveable Paddle1Moveable => _paddle1Moveable;
    
    private readonly Moveable _paddle2Moveable;
    public Moveable Paddle2Moveable => _paddle2Moveable;
    
    public Ball(Moveable moveable, Vector3 direction, ref Moveable paddle1Moveable, ref Moveable paddle2Moveable)
    {
        Moveable = moveable;
        Direction = direction;
        _paddle1Moveable = paddle1Moveable;
        _paddle2Moveable = paddle2Moveable;
    }

    public void Move(ref Bounds terrainBounds, float delta)
    {
        Moveable.Move(Direction.normalized * delta);
        if (Moveable.Bounds.center.x < terrainBounds.center.x - terrainBounds.size.x)
            Moveable.Bounds.center.Set(terrainBounds.center.x - terrainBounds.size.x, Moveable.Bounds.center.y, Moveable.Bounds.center.z);
        if (Moveable.Bounds.center.x > terrainBounds.center.x + terrainBounds.size.x)
            Moveable.Bounds.center.Set(terrainBounds.center.x + terrainBounds.size.x, Moveable.Bounds.center.y, Moveable.Bounds.center.z);
    }
}