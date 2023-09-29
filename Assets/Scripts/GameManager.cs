using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pong;
using MCTS;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum PlayerType
    {
        Human,Random,PseudoRandom,MonteCarlo
    }
    private GameState _gameState;

    public GameState GameState => _gameState;

    private PlayerType _p1Type;
    private PlayerType _p2Type;

    public PlayerType P1Type
    {
        get => _p1Type;
        set => _p1Type = value;
    }

    public PlayerType P2Type
    {
        get => _p2Type;
        set => _p2Type = value;
    }

    private APlayer Player1;
    private APlayer Player2;

    [Header("Reference")]
    public GameObject paddleGo1;
    public GameObject paddleGo2;
    public GameObject ballGo;
    
    [Header("Initial Locations")]
    public Vector3 paddle1InitialLocation;
    public Vector3 paddle2InitialLocation;
    public Vector3 ballInitialLocation;
    
    public float initialTimer = 60f;
    
    [SerializeField] 
    public Bounds terrainBounds;

    private bool _isGameRunning;
    
    public AudioSource pongSound;
    
    void Start()
    {
        _isGameRunning = false;
    }
    private void SetPlayers()
    {
        Player1 = NewPlayer(_p1Type, true);
        if (! (_p1Type == _p2Type && _p1Type == PlayerType.Human))
            Player2 = NewPlayer(_p2Type, false);
        else
            Player2 = new Player(false);
    }
    private APlayer NewPlayer(PlayerType t,bool isP1)
    {
        APlayer i = t switch
        {
            PlayerType.Human => new Player(isP1),
            PlayerType.Random => new RandomPlayer(isP1),
            PlayerType.PseudoRandom => new PseudoRandomPlayer(isP1),
            PlayerType.MonteCarlo => new MCTSPlayer(isP1),
            _ => throw new ArgumentOutOfRangeException(nameof(t), t, null)
        };
        return i;
    }



    void ResetGameState(Vector3 direction)
    {
        var paddle1 = new Paddle( new Moveable(4f, paddleGo1.transform.position,paddleGo1.transform.GetChild(0).localScale));
        var paddle2 = new Paddle( new Moveable(4f, paddleGo2.transform.position,paddleGo2.transform.GetChild(0).localScale));
        var ball = new Ball(new Moveable(4f, ballGo.transform.position, ballGo.transform.lossyScale),
            direction,paddle1.Moveable,paddle2.Moveable);
        _gameState = new GameState(paddle1, paddle2, ball, terrainBounds, initialTimer);
    }
    
    public void InitializeGame()
    {
        ResetGameState(new Vector3(-1f,0,-1f));
        SetPlayers();
        BuildWalls();
        _isGameRunning = true;
    }
    
    void ResetGame()
    {
        paddleGo1.transform.position = paddle1InitialLocation;
        paddleGo2.transform.position = paddle2InitialLocation;
        ballGo.transform.position = ballInitialLocation;
        
        Vector3 direction = _gameState.Ball.Direction;
        direction*= -1;
        
        TrailRenderer trail = ballGo.GetComponent<TrailRenderer>();
        trail.Clear();
        
        ResetGameState(direction);
        
        _isGameRunning = true;
    }

    void Update()
    {
        //To-do find a way to update bot players game states
        if (_isGameRunning)
        {
            var dir = _gameState.Ball.Direction;
            _gameState.Tick(Player1.GetAction(ref this._gameState), Player2.GetAction(ref this._gameState), Time.deltaTime);
            SyncMovables();
            
            if (_gameState.Ball.Direction != dir) pongSound.Play();

            if (_gameState.GameStatus != GameState.GameStatusEnum.Ongoing)
            {
                _isGameRunning = false;
            }
        }else if(Input.GetKeyDown(KeyCode.Return))
        {
            ResetGame();
        }
    }
    
    void SyncMovables()
    {
        paddleGo1.transform.position = _gameState.Paddle1.Moveable.Bounds.center;
        paddleGo2.transform.position = _gameState.Paddle2.Moveable.Bounds.center;
        ballGo.transform.position = _gameState.Ball.Moveable.Bounds.center;
    }

    private void BuildWalls()
    {
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
    
}
