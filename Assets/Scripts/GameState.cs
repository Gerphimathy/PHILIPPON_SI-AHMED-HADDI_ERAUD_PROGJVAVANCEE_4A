using System;
using UnityEngine;
using UnityEngine.Assertions;
using Action = Pong.Action;

public class GameState

{

    private Paddle _paddle1;
    private Paddle _paddle2;
    private Ball _ball;
    private Bounds _terrainBounds;

    public enum GameStatusEnum
    {
        Player1Win = -1,
        Ongoing = 0,
        Player2Win
    }

    private GameStatusEnum _gameStatus;

    public GameStatusEnum GameStatus => _gameStatus;


    public GameState(Paddle paddle1, Paddle paddle2, Ball ball, Bounds terrainBounds)
    {
        _paddle1 = paddle1;
        _paddle2 = paddle2;
        _ball = ball;
        _terrainBounds = terrainBounds;
        _gameStatus = GameStatusEnum.Ongoing;
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
        if (!_terrainBounds.Contains(_ball.Moveable.Bounds.center)) _ball.Direction.x *= -1f;

        if (_ball.Moveable.Bounds.Intersects(_paddle1.Moveable.Bounds) ||
            _ball.Moveable.Bounds.Intersects(_paddle2.Moveable.Bounds))
        {
            _ball.Direction.z *= -1f;
        }

        _ball.Move(ref _terrainBounds, delta);
        
        
        if (!_terrainBounds.Intersects(_ball.Moveable.Bounds))
        {
            var ballCenter = _ball.Moveable.Bounds.center;
            var closestPoint = _terrainBounds.ClosestPoint(ballCenter);

            var difference = ballCenter.z - closestPoint.z;

            if (difference > 0.01) _gameStatus = GameStatusEnum.Player2Win;
            else if (difference < 0.01) _gameStatus = GameStatusEnum.Player1Win;
            
            Debug.Log(_gameStatus);

        }
    }
    
}
