using System;
using Pong;
using UnityEngine;
using UnityEngine.Assertions;
using Action = Pong.Action;

public struct Paddle
{
   
   private Moveable _moveable;

    public Paddle(Moveable moveable)
    {
        _moveable = moveable;
    }

    public Moveable Moveable => _moveable;

   public void Move(ref Bounds terrainBounds, Action agentInput, float delta)
   {
      switch (agentInput)
      {
         case Action.Up:
            _moveable.Move(Vector3.right * delta);
            if(
               !terrainBounds.Contains(_moveable.Bounds.min)
               ||
               !terrainBounds.Contains(_moveable.Bounds.max)
               ) _moveable.Move(Vector3.left * delta);
            break;
         
         case Action.Down:
            _moveable.Move(Vector3.left * delta);
            if(
               !terrainBounds.Contains(_moveable.Bounds.min)
               ||
               !terrainBounds.Contains(_moveable.Bounds.max)
            ) _moveable.Move(Vector3.right * delta);
            break;
         
         case Action.None:
            break;
         
         default:
            Assert.IsFalse(true);
            break;
      }
   }
}
