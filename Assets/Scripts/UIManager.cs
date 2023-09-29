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

    public void StartGame()
    {
        gameManager.InitializeGame();
        mainMenuPanel.SetActive(false);
        
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
    }
    
    public void OnMainMenuButtonPressed()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

}
