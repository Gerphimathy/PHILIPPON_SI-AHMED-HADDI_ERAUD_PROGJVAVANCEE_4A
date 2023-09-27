using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pong;
using UnityEngine.SceneManagement;

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
        var paddle1 = new Paddle( new Moveable(4f, paddleGo1.transform.position,paddleGo1.transform.GetChild(0).localScale));
        var paddle2 = new Paddle( new Moveable(4f, paddleGo2.transform.position,paddleGo2.transform.GetChild(0).localScale));
        var ball = new Ball(new Moveable(4f, ballGo.transform.position, ballGo.transform.lossyScale),
            new Vector3(-1f,0,-1f),paddle1.Moveable,paddle2.Moveable);
        SetPlayers();
        _gameState = new GameState(paddle1, paddle2, ball, terrainBounds);
        
        //Create cube object walls that match _terrainBounds
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = new Vector3(terrainBounds.center.x,terrainBounds.center.y,terrainBounds.min.z);
        wall.transform.localScale = new Vector3(terrainBounds.size.x,2,0.1f);
        
        GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall2.transform.position = new Vector3(terrainBounds.center.x,terrainBounds.center.y,terrainBounds.max.z);
        wall2.transform.localScale = new Vector3(terrainBounds.size.x,2,0.1f);
        
        GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall3.transform.position = new Vector3(terrainBounds.min.x,terrainBounds.center.y,terrainBounds.center.z);
        wall3.transform.localScale = new Vector3(0.1f,2,terrainBounds.size.z);
        
        GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall4.transform.position = new Vector3(terrainBounds.max.x,terrainBounds.center.y,terrainBounds.center.z);
        wall4.transform.localScale = new Vector3(0.1f,2,terrainBounds.size.z);
    }
    private void SetPlayers()
    {
        Player1 = new Player(Player.Scheme.Arrow);
        Player2 = new RandomPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_gameState.Ball.Moveable.Bounds.center,_gameState.Ball.Moveable.Bounds.size);
        Gizmos.DrawCube(_gameState.Paddle1.Moveable.Bounds.center,_gameState.Paddle1.Moveable.Bounds.size);
        Gizmos.DrawCube(_gameState.Paddle2.Moveable.Bounds.center,_gameState.Paddle2.Moveable.Bounds.size);
        Gizmos.DrawCube(_gameState.TerrainBounds.center,_gameState.TerrainBounds.size);

    }

    void Update()
    {
        //To-do find a way to update bot players game states

        //
        _gameState.Tick(Player1.GetAction(), Player2.GetAction(), Time.deltaTime);
        SyncMovables();

        if (_gameState.GameStatus != GameState.GameStatusEnum.Ongoing)
        {
            Destroy(ballGo.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    void SyncMovables()
    {
        paddleGo1.transform.position = _gameState.Paddle1.Moveable.Bounds.center;
        paddleGo2.transform.position = _gameState.Paddle2.Moveable.Bounds.center;
        ballGo.transform.position = _gameState.Ball.Moveable.Bounds.center;
    }
}
