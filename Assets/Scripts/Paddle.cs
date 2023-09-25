using System;

public struct Paddle
{
   
   private Moveable _moveable;

   public Moveable Moveable => _moveable;

   public void Move(Action agentInput, float delta)
   {
      //TODO: Implement Paddle Movement
      throw new NotImplementedException();
   }
}
