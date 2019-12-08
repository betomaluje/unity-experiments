﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject youWinPanel;    

    [SerializeField] private TextMeshProUGUI gameOverMaxScoreObject;
    [SerializeField] private TextMeshProUGUI youWinMaxScoreObject;

    public void GameOver(int maxScore)
    {
        gameOverPanel.SetActive(true);
        gameOverMaxScoreObject.SetText("Max score: " + maxScore);
        TogglePlayersActive(false);
    }

    public void YouWin(int maxScore) 
    {
        youWinPanel.SetActive(true);
        youWinMaxScoreObject.SetText("Max score: " + maxScore);
        TogglePlayersActive(false);
    }

    private void TogglePlayersActive(bool enable)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = enable;
            }
            PlayerGrabbing playerGrabbing = player.GetComponent<PlayerGrabbing>();
            if (playerGrabbing != null)
            {
                playerGrabbing.enabled = enable;
            }
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
}
