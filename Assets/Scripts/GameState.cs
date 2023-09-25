using UnityEngine;
using UnityEngine.Assertions;

using Pong;

public class GameState

{

    private Paddle _paddle1;
    private Paddle _paddle2;
    private Ball _ball;
    private Bounds _terrainBounds;

    private bool _hasGameEnded;

    public bool HasGameEnded => _hasGameEnded;

    public GameState(Paddle paddle1, Paddle paddle2, Ball ball, Bounds terrainBounds)
    {
        _paddle1 = paddle1;
        _paddle2 = paddle2;
        _ball = ball;
        _terrainBounds = terrainBounds;
    }

    public Paddle Paddle1 => _paddle1;

    public Paddle Paddle2 => _paddle2;

    public Ball Ball => _ball;

    public Bounds TerrainBounds => _terrainBounds;


    public void Tick(Action actionAgent1, Action actionAgent2, float delta)
    {
        _paddle1.Move(actionAgent1, delta);
        _paddle2.Move(actionAgent2, delta);
        
        _ball.Move(ref _terrainBounds, delta);

        if (_terrainBounds.Intersects(_ball.Moveable.Bounds)) _hasGameEnded = true;
    }
    
    //TODO: 
}
