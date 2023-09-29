using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Action = Pong.Action;

public struct GameState

{
    private static readonly NativeArray<Action> ActionList = new (3, Allocator.Persistent) {
        [0] = Action.Up, [1] = Action.Down, [2]= Action.None
    };

    
    /// <summary>
    /// Emplacements mémoire réservés pour utilisation de Burst pour la méthode GetPossibleAction
    /// </summary>
    private static NativeArray<Action> ActionBufferZero = new(0, Allocator.Persistent);

    private static NativeArray<Action> ActionBufferOne = new(1, Allocator.Persistent)
    {
        [0] = Action.None
    };
    
    private static NativeArray<Action> ActionBufferTwo = new(2, Allocator.Persistent)
    {
        [0] = Action.None, [1] = Action.None
    };
    
    
    private static NativeArray<Action> ActionBufferThree = new(3, Allocator.Persistent)
    {
        [0] = Action.None, [1] = Action.None, [2] = Action.None
    };
    
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

    public GameState(Paddle paddle1, Paddle paddle2, float ballSpeed, Vector3 ballInitialLocation, Vector3 ballBounds, Vector3 direction,
        Bounds terrainBounds, float timer)
    {
        _paddle1 = paddle1;
        _paddle2 = paddle2;
        var paddle1Moveable = _paddle1.Moveable;
        var paddle2Moveable = _paddle2.Moveable;
        _ball = new Ball(new Moveable(ballSpeed, ballInitialLocation, ballBounds),
            direction,ref paddle1Moveable,ref paddle2Moveable);
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
    
    [BurstCompile(CompileSynchronously = true)]
    public NativeArray<Action> GetPossibleActions(bool player, float delta)
    {
        Assert.IsTrue(ActionList.IsCreated);
        int j = 0;
        for (int i = 0; i < ActionList.Length; i++)
        {
            if (isActionValid(ActionList[i], player, delta))
            {
                ActionBufferThree[j] = ActionList[i];
                switch (j)
                {
                    case 0:
                        ActionBufferOne[j] = ActionList[i];
                        goto case 1;
                    case 1:
                        ActionBufferTwo[j] = ActionList[i];
                        goto case 2;
                    case 2:
                        ActionBufferThree[j] = ActionList[i];
                        break;
                }
                ++j;
            }
        }

        return j switch
        {
            0 => ActionBufferZero,
            1 => ActionBufferOne,
            2 => ActionBufferTwo,
            3 => ActionBufferThree,
            _ => ActionBufferZero
        };
    }
    public bool isActionValid(Action a, bool player, float delta)
    {
        if (a == Action.None)
            return true;
        //Copied version because struct, we can move it no problem;
        Moveable target = (player ? Paddle1 : Paddle2).Moveable;
        Vector3 leftDelta = Vector3.left * delta;
        if (a == Action.Up)
        {
            target.Move(-leftDelta);
            return _terrainBounds.max.x > target.Bounds.max.x;
        }
        if (a == Action.Down)
        {
            target.Move(leftDelta);
            return _terrainBounds.min.x < target.Bounds.min.x;
        }
        throw new Exception("Unsupported action");
    }
    public void Tick(Action actionAgent1, Action actionAgent2, float delta)
    {
        //if (isActionValid(actionAgent1, true, delta))
            _paddle1.Move(ref _terrainBounds, actionAgent1, delta);
        //if (isActionValid(actionAgent2, false, delta))
            _paddle2.Move(ref _terrainBounds, actionAgent2, delta);

        _ball.Move(ref _terrainBounds, delta);
        if (!_terrainBounds.Contains(_ball.Moveable.Bounds.center + Vector3.Scale(_ball.Direction.normalized, _ball.Moveable.Bounds.size)))
            _ball.Direction.x *= -1f;


        ref var paddle1Bounds = ref _paddle1.Moveable.Bounds;
        ref var paddle2Bounds = ref _paddle2.Moveable.Bounds;
        ref var ballBounds = ref _ball.Moveable.Bounds;

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

            if (difference > 0.02) _gameStatus = GameStatusEnum.Player2Win;
            else if (difference < 0.02) _gameStatus = GameStatusEnum.Player1Win;

            //Debug.Log(_gameStatus);

        }
    }

}
