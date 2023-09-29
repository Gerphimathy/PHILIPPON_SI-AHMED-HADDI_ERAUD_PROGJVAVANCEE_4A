using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text victoryText;

    public void Start()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        gameUI.SetActive(false);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        gameUI.SetActive(true);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
        gameManager.InitializeGame();
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }


    public void OnPlayerOneDropDownChanged(TMP_Dropdown change)
    {
        gameManager.P1Type = (GameManager.PlayerType)change.value;
        Debug.Log(gameManager.P1Type);
    }
    
    public void OnPlayerTwoDropDownChanged(TMP_Dropdown change)
    {
        gameManager.P2Type = (GameManager.PlayerType)change.value;
        Debug.Log(gameManager.P2Type);
    }

    public void OnOptionsButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        gameUI.SetActive(false);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }
    
    public void OnMainMenuButtonPressed()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        gameUI.SetActive(false);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void DisplayVictoryMenu(bool isP1)
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        gameUI.SetActive(false);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(true);
        victoryText.text = "Player " + (isP1 ? "1" : "2") + " has won the game";
    }

    public void ActivatePausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void DeactivatePausePanel()
    {
        pausePanel.SetActive(false);
    }

}
