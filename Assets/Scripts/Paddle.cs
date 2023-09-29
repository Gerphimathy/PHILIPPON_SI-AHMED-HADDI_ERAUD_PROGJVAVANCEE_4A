using System;
using Pong;
using UnityEngine;
using UnityEngine.Assertions;
using Action = Pong.Action;

public struct Paddle
{
   
   public Moveable Moveable;

    public Paddle(Moveable moveable)
    {
       Moveable = moveable;
    }


   public void Move(ref Bounds terrainBounds, Action agentInput, float delta)
   {
      switch (agentInput)
      {
         case Action.Up:
            Moveable.Move(Vector3.right * delta);
            if(
               !terrainBounds.Contains(Moveable.Bounds.min)
               ||
               !terrainBounds.Contains(Moveable.Bounds.max)
               ) Moveable.Move(Vector3.left * delta);
            break;
         
         case Action.Down:
            Moveable.Move(Vector3.left * delta);
            if(
               !terrainBounds.Contains(Moveable.Bounds.min)
               ||
               !terrainBounds.Contains(Moveable.Bounds.max)
            ) Moveable.Move(Vector3.right * delta);
            break;
         
         case Action.None:
            break;
         
         default:
            Assert.IsFalse(true);
            break;
      }
   }
}
