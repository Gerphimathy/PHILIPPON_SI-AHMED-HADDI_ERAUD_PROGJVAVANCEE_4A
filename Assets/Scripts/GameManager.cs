using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pong;

public class GameManager : MonoBehaviour
{
    private GameState _gameState;
    
    public IPlayer Player1;
    public IPlayer Player2;

    public GameObject paddleGo1;
    public GameObject paddleGo2;
    public GameObject ballGo;
    
    [SerializeField] 
    public Bounds terrainBounds;
    
    void Start()
    {
        var paddle1 = new Paddle();
        var paddle2 = new Paddle();
        var ball = new Ball();
        
        _gameState = new GameState(paddle1, paddle2, ball, terrainBounds);
    }

    void Update()
    {
        _gameState.Tick(Player1.GetAction(), Player2.GetAction(), Time.deltaTime);
        SyncMovables();   
    }
    
    void SyncMovables()
    {
        paddleGo1.transform.position = _gameState.Paddle1.Moveable.Bounds.center;
        paddleGo2.transform.position = _gameState.Paddle2.Moveable.Bounds.center;
        ballGo.transform.position = _gameState.Ball.Moveable.Bounds.center;
    }
}
