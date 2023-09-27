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
        var paddle1 = new Paddle( new Moveable(4f, paddleGo1.transform.position,paddleGo1.transform.lossyScale));
        var paddle2 = new Paddle( new Moveable(4f, paddleGo2.transform.position,paddleGo2.transform.lossyScale));
        var ball = new Ball(new Moveable(4f, ballGo.transform.position, ballGo.transform.lossyScale),
            new Vector3(-1f,0,-1f),paddle1.Moveable,paddle2.Moveable);
        SetPlayers();
        _gameState = new GameState(paddle1, paddle2, ball, terrainBounds);
    }
    private void SetPlayers()
    {
        Player1 = new Player(Player.Scheme.Arrow);
        Player2 = new RandomPlayer();
    }

    void Update()
    {
        //To-do find a way to update bot players game states

        //
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
