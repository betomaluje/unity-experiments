using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TMP_Text mText;
    [SerializeField] private GameEvent gameOverEvent;

    private int currentAmount = 0;
    private int maxScore = 0;

    private void Update()
    {        
        mText.text = currentAmount.ToString();
    }

    public void Reset()
    {
        currentAmount = 0;
        maxScore = 0;
    }

    public void Add(GameObject go)
    {
        currentAmount++;
        Debug.Log("adding: " + currentAmount);

        if (currentAmount > maxScore)
        {
            maxScore = currentAmount;
        }

    }

    public void Substract(float amount)
    {
        Debug.Log(currentAmount + " -> damage: " + Mathf.RoundToInt(amount));
        currentAmount -= Mathf.RoundToInt(amount);
        Debug.Log("currentAmount: " + currentAmount);

        if (currentAmount < 0)
        {
            gameOverEvent.sentInt = maxScore;
            gameOverEvent.Raise();
            Reset();
        }
    }
}
