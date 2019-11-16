using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject youWinPanel;

    [SerializeField] private TextMeshProUGUI gameOverMaxScoreObject;
    [SerializeField] private TextMeshProUGUI youWinMaxScoreObject;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerGrabbing playerGrabbing;

    public void GameOver(int maxScore)
    {
        gameOverPanel.SetActive(true);
        gameOverMaxScoreObject.SetText("Max score: " + maxScore);
        TogglePlayers(false);
    }

    public void YouWin(int maxScore) 
    {
        youWinPanel.SetActive(true);
        youWinMaxScoreObject.SetText("Max score: " + maxScore);
        TogglePlayers(false);
    }

    private void TogglePlayers(bool enable)
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
