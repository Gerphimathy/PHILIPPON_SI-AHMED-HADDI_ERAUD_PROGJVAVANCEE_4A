using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Paddle : MonoBehaviour
{
    public enum Controls
    {
        Player,
        AI,
        Random
    }
    public enum MovementAxis
    {
        X,Y,Z
    } 
    public MovementAxis movementAxis;
    
    public float speed;
    
    
    public Controls control;
    public Bounds bounds;

    public void Move(int direction)
    {
        switch (movementAxis)
        {
            case MovementAxis.X:
                transform.Translate(direction * speed * Time.deltaTime, 0, 0);
                break;
            case MovementAxis.Y:
                transform.Translate(0, direction * speed * Time.deltaTime, 0);
                break;
            case MovementAxis.Z:
                transform.Translate(0, 0, direction * speed * Time.deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
