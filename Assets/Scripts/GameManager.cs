using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pong;
using MCTS;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private MCTSSettings mctsSettings;
    
    public enum PlayerType
    {
        Human,Random,PseudoRandom,MonteCarlo
    }
    private GameState _gameState;

    public GameState GameState => _gameState;

    private int _player1Score;
    private int _player2Score;
    
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
    public TMP_Text scoreText;
    
    [Header("Initial Locations")]
    public Vector3 paddle1InitialLocation;
    public Vector3 paddle2InitialLocation;
    public Vector3 ballInitialLocation;
    
    public float initialTimer = 60f;
    
    [SerializeField] 
    public Bounds terrainBounds;

    private bool _isGameRunning;
    
    public AudioSource pongSound;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField] private int maxScoreToWin;

    
    void Start()
    {
        _isGameRunning = false;
        _player1Score = 0;
        _player2Score = 0;
        BuildWalls();
    }
    private void SetPlayers()
    {
        Player1 = NewPlayer(_p1Type, true);
        if (! (_p1Type == _p2Type && _p1Type == PlayerType.Human))
            Player2 = NewPlayer(_p2Type, false);
        else
            Player2 = new Player(false);
        
        Assert.IsTrue(Player1 != null);
        Assert.IsTrue(Player2 != null);
        
        if(Player1 is MCTSPlayer mctsPlayer)
            mctsPlayer.SetSettings(mctsSettings);
        if(Player2 is MCTSPlayer mctsPlayer2)
            mctsPlayer2.SetSettings(mctsSettings);
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
        var paddle1 = new Paddle( new Moveable(4f, paddle1InitialLocation,paddleGo1.transform.GetChild(0).localScale));
        var paddle2 = new Paddle( new Moveable(4f, paddle2InitialLocation,paddleGo2.transform.GetChild(0).localScale));
        var ball = new Ball(new Moveable(4f, ballInitialLocation, ballGo.transform.lossyScale),
            direction,paddle1.Moveable,paddle2.Moveable);
        _gameState = new GameState(paddle1, paddle2, ball, terrainBounds, initialTimer);
    }
    
    public void InitializeGame()
    {
        _player1Score = 0;
        _player2Score = 0;
        Assert.IsTrue(_player1Score == 0);
        Assert.IsTrue(_player2Score == 0);
        UpdateScore();
        ResetGameState(new Vector3(-1f,0,-1f));
        Assert.IsTrue(_gameState.GameStatus == GameState.GameStatusEnum.Ongoing);
        SetPlayers();
        _isGameRunning = true;
        Assert.IsTrue(Player1 != null);
        Assert.IsTrue(Player2 != null);
    }
    
    void ResetGame()
    {
        paddleGo1.transform.position = paddle1InitialLocation;
        paddleGo2.transform.position = paddle2InitialLocation;
        ballGo.transform.position = ballInitialLocation;
        
        TrailRenderer trail = ballGo.GetComponent<TrailRenderer>();
        trail.Clear();
        
        Vector3 direction = _gameState.Ball.Direction;
        direction*= -1;
        
        
        ResetGameState(direction);
        
        _isGameRunning = true;
    }

    void Update()
    {
        //To-do find a way to update bot players game states
        if (_isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isGameRunning = false;
                uiManager.ActivatePausePanel();
            }
            
            var dir = _gameState.Ball.Direction;
            _gameState.Tick(Player1.GetAction(ref this._gameState), Player2.GetAction(ref this._gameState), Time.deltaTime);
            SyncMovables();
            
            if (_gameState.Ball.Direction != dir) pongSound.Play();

            if (_gameState.GameStatus != GameState.GameStatusEnum.Ongoing)
            {
                _isGameRunning = false;
                
                if(_gameState.GameStatus == GameState.GameStatusEnum.Player1Win)
                    _player1Score++;
                else if(_gameState.GameStatus == GameState.GameStatusEnum.Player2Win)
                    _player2Score++;
                else
                    Assert.IsTrue(_gameState.GameStatus == GameState.GameStatusEnum.Draw);
                
                UpdateScore();

                if (_player1Score < maxScoreToWin && _player2Score < maxScoreToWin)
                {
                    ResetGame();
                }
                else 
                {
                    uiManager.DisplayVictoryMenu(_player1Score >= maxScoreToWin);
                }
            }
        }
    }
    
    void SyncMovables()
    {
        paddleGo1.transform.position = _gameState.Paddle1.Moveable.Bounds.center;
        paddleGo2.transform.position = _gameState.Paddle2.Moveable.Bounds.center;
        ballGo.transform.position = _gameState.Ball.Moveable.Bounds.center;
    }

    void UpdateScore()
    {
        scoreText.text = _player1Score + " - " + _player2Score;
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

    public void ResumeGame()
    {
        _isGameRunning = true;
        uiManager.DeactivatePausePanel();
    }
    
}
