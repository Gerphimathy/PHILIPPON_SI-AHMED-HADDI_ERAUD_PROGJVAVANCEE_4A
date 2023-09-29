using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Action = Pong.Action;

public struct GameState

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

    public readonly float InitialTimer;
    public GameState(Paddle paddle1, Paddle paddle2, Ball ball, Bounds terrainBounds, float timer)
    {
        _paddle1 = paddle1;
        _paddle2 = paddle2;
        _ball = ball;
        _terrainBounds = terrainBounds;
        _gameStatus = GameStatusEnum.Ongoing;
        InitialTimer = timer;
        _timer = InitialTimer;
    }

    public GameState(GameState gameState) : this(gameState._paddle1,gameState._paddle2,gameState._ball,gameState.TerrainBounds,gameState.InitialTimer)
    {
        //We keep initial timer only we don't intialize by copy
        this._timer = gameState.Timer;
    }

    public Paddle Paddle1 => _paddle1;

    public Paddle Paddle2 => _paddle2;

    public Ball Ball => _ball;

    public Bounds TerrainBounds => _terrainBounds;
    public List<Action> GetPossibleActions(bool player, float? delta = null)
    {
        var l = new List<Action>();
        if (!delta.HasValue)
            delta = Time.deltaTime;
        var actions = (Action[])Enum.GetValues(typeof(Action));
        for (int i = 0; i < actions.Length; i++)
        {
            if (isActionValid(actions[i], player, delta.Value))
                l.Add(actions[i]);
        }
        return l;
    }
    public bool isActionValid(Action a, bool player, float delta)
    {
        if (a == Action.None)
            return true;
        //Copied version because struct, we can move it no problem;
        Moveable target = (player ? Paddle1 : Paddle2).Moveable;
        if (a == Action.Up)
        {
            target.Move(Vector3.right * delta);
            bool valid = (_terrainBounds.Contains(target.Bounds.min)
               &&
               _terrainBounds.Contains(target.Bounds.max));
            target.Move(Vector3.left * delta);
            return valid;
        }
        if (a == Action.Down)
        {
            target.Move(Vector3.left * delta);
            bool valid = (_terrainBounds.Contains(target.Bounds.min)
               &&
               _terrainBounds.Contains(target.Bounds.max));
            target.Move(Vector3.right * delta);
            return valid;
        }
        throw new Exception("Unsupported action");
    }
    public void Tick(Action actionAgent1, Action actionAgent2, float delta)
    {
        if (isActionValid(actionAgent1, true, delta))
            _paddle1.Move(ref _terrainBounds, actionAgent1, delta);
        if (isActionValid(actionAgent2, false, delta))
            _paddle2.Move(ref _terrainBounds, actionAgent2, delta);

        _ball.Move(ref _terrainBounds, delta);
        if (!_terrainBounds.Contains(_ball.Moveable.Bounds.center + Vector3.Scale(_ball.Direction.normalized, _ball.Moveable.Bounds.size)))
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
        
        if (!_terrainBounds.Contains(_ball.Moveable.Bounds.center) && _gameStatus == GameStatusEnum.Ongoing)
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
