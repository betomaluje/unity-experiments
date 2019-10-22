using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI maxScoreObject;

    public void GameOver(int maxScore)
    {
        gameOverPanel.SetActive(true);
        maxScoreObject.SetText("Max score: " + maxScore);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
