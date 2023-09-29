using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public struct Moveable
{
    private float _speed;
    public Bounds Bounds;

    public Moveable(float speed=1f) : this(speed,Vector3.zero,Vector3.one)
    {
    }

    public Moveable(float speed, Vector3 boundsPosition, Vector3 boundsSize)
    {
        _speed = speed;
        Bounds = new Bounds();
        Bounds.center = boundsPosition;
        Bounds.size = boundsSize;
    }

    public float Speed => _speed;


    public void Move(Vector3 displacement)
    {
        Bounds.center += displacement * _speed;
    }

}
