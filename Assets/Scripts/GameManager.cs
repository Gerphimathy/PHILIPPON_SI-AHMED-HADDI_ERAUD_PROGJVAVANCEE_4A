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
    
    void Start()
    {
        _gameState = new GameState();
    }

    void Update()
    {
        _gameState.Tick(GetAction(player1Agent), GetAction(player2Agent), Time.deltaTime);
        SyncMovables();   
    }
    
    void SyncMovables()
    {
        
    }

    Action GetAction(Agent agent)
    {
        switch (agent)
        {
            case Agent.Player1Agent:
                if (Input.GetKey(KeyCode.Z))
                    return Action.Up;
                else if (Input.GetKey(KeyCode.S))
                    return Action.Down;
                else 
                    return Action.None;
                break;
            
            case Agent.Player2Agent:
                if (Input.GetKey(KeyCode.UpArrow))
                    return Action.Up;
                else if (Input.GetKey(KeyCode.DownArrow))
                    return Action.Down;
                else 
                    return Action.None;
                break;
            
            case Agent.RandomAgent:
                return (Action)Random.Range(-1, 2);
            break;
            default:
                return Action.None;
        }
    }
}
