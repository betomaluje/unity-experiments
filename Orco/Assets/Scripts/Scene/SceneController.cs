using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject youWinPanel;    

    public void GameOver(int maxScore)
    {
        TogglePlayersActive(false);
        gameOverPanel.SetActive(true);
    }

    public void YouWin(int maxScore) 
    {
        TogglePlayersActive(false);
        youWinPanel.SetActive(true);
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
            PlayerInput playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null) 
            {
                playerInput.enabled = enable;
            }
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
