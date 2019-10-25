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
        playerMovement.enabled = false;
        playerGrabbing.enabled = false;
    }

    public void YouWin(int maxScore) 
    {
        youWinPanel.SetActive(true);
        youWinMaxScoreObject.SetText("Max score: " + maxScore);
        playerMovement.enabled = false;
        playerGrabbing.enabled = false;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
