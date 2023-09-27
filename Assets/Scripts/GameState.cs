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

    private float _timer;

    public float Timer => _timer;

    public enum GameStatusEnum
    {
        Player1Win = -1,
        Ongoing = 0,
        Player2Win = 1,
        Draw = 2
    }

    private GameStatusEnum _gameStatus;

    public GameStatusEnum GameStatus => _gameStatus;


    public GameState(Paddle paddle1, Paddle paddle2, Ball ball, Bounds terrainBounds, float timer)
    {
        _paddle1 = paddle1;
        _paddle2 = paddle2;
        _ball = ball;
        _terrainBounds = terrainBounds;
        _gameStatus = GameStatusEnum.Ongoing;
        _timer = timer;
    }

    public Paddle Paddle1 => _paddle1;

    public Paddle Paddle2 => _paddle2;

    public Ball Ball => _ball;

    public Bounds TerrainBounds => _terrainBounds;


    public void Tick(Action actionAgent1, Action actionAgent2, float delta)
    {
        _paddle1.Move(ref _terrainBounds, actionAgent1, delta);
        _paddle2.Move(ref _terrainBounds, actionAgent2, delta);
        
        _ball.Move(ref _terrainBounds, delta);
        if (!_terrainBounds.Contains(_ball.Moveable.Bounds.center + Vector3.Scale(_ball.Direction.normalized,_ball.Moveable.Bounds.size)))
            _ball.Direction.x *= -1f;

        
        var paddle1Bounds = _paddle1.Moveable.Bounds;
        var paddle2Bounds = _paddle2.Moveable.Bounds;
        var ballBounds = _ball.Moveable.Bounds;
        
        if (ballBounds.Intersects(paddle1Bounds))
        {
            if (Math.Abs(ballBounds.center.x - paddle1Bounds.center.x) > paddle1Bounds.extents.x + ballBounds.extents.x)
                _ball.Direction.x *= -1f;
            else
                _ball.Direction.z *= -1f;
        }

        if (ballBounds.Intersects(paddle2Bounds))
        {
            
            if (Math.Abs(ballBounds.center.x - paddle2Bounds.center.x) > paddle2Bounds.extents.x + ballBounds.extents.x)
                _ball.Direction.x *= -1f;
            else
                _ball.Direction.z *= -1f;
        }
        
        _ball.Move(ref _terrainBounds, delta);
        
        _timer -= delta;
        if (_timer <= 0) _gameStatus = GameStatusEnum.Draw;
        
        if (!_terrainBounds.Intersects(_ball.Moveable.Bounds) && _gameStatus == GameStatusEnum.Ongoing)
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
