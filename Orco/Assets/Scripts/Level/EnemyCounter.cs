using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text humanScoreText;
    [SerializeField] private float clearStagePercentage = 0.5f;
    [SerializeField] private GameEvent appearTrophyEvent;

    private float totalHumans = 0;
    private float currentHumansKilled = 0;
    private bool trophyAppear = false;
    
    void Update()
    {
        humanScoreText.text = currentHumansKilled.ToString() + "/" + totalHumans.ToString();

        float currentPercentage = totalHumans > 0 ? currentHumansKilled / totalHumans : 0f;

        if (!trophyAppear && currentPercentage >= clearStagePercentage)
        {
            trophyAppear = true;
            appearTrophyEvent.Raise();
        }
    }

    public void AddTotalHumans()
    {
        totalHumans += 1f;
    }

    public void AddKill()
    {
        currentHumansKilled += 1f;
    }
}
