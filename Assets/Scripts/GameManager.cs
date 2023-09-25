using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pong;

public class GameManager : MonoBehaviour
{
    private GameState _gameState;
    
    public Agent player1Agent;
    public Agent player2Agent;

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
        _gameState.Tick(GetAction(player1Agent), GetAction(player2Agent), Time.deltaTime);
        SyncMovables();   
    }
    
    void SyncMovables()
    {
        paddleGo1.transform.position = _gameState.Paddle1.Moveable.Bounds.center;
        paddleGo2.transform.position = _gameState.Paddle2.Moveable.Bounds.center;
        ballGo.transform.position = _gameState.Ball.Moveable.Bounds.center;
    }

    Action GetAction(Agent agent)
    {
        switch (agent)
        {
            case Agent.Player1Agent:
                if (Input.GetKey(KeyCode.Z))
                    return Action.Up;
                if (Input.GetKey(KeyCode.S))
                    return Action.Down;
                return Action.None;
            
            case Agent.Player2Agent:
                if (Input.GetKey(KeyCode.UpArrow))
                    return Action.Up;
                if (Input.GetKey(KeyCode.DownArrow))
                    return Action.Down;
                 
                return Action.None;
            
            case Agent.RandomAgent:
                return (Action)Random.Range(-1, 2);
            
            case Agent.MctsAgent:
                //TODO
                return Action.None;
            
            default:
                return Action.None;
        }
    }
}
