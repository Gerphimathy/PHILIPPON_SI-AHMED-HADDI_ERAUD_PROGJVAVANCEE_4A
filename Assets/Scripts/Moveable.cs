using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Moveable
{
    private float _speed;
    private Bounds _bounds;

    public Moveable(float speed, Vector3 boundsPosition, Vector3 boundsSize)
    {
        _speed = speed;
        _bounds = new Bounds();
        _bounds.center = boundsPosition;
        _bounds.size = boundsSize;
    }

    public float Speed => _speed;

    public Bounds Bounds => _bounds;

    public void Move(Vector3 displacement)
    {
        _bounds.center += displacement;
    }

}
