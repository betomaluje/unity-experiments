using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int lives = 3;
    [SerializeField] private GameEvent gameOverEvent;
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite damageSprite;
    [SerializeField] private Sprite normalSprite;

    private int currentAmount = 0;
    private int maxScore = 0;
    private int currentLives = 0;

    void Start()
    {
        currentLives = lives;
    }

    private void Update()
    {        
        scoreText.text = currentAmount.ToString();
    }

    public void Reset()
    {
        currentAmount = 0;
        maxScore = 0;
        currentLives = lives;

        foreach (var image in images)
        {
            image.sprite = normalSprite;
        }
    }

    public void AddScore()
    {       
        currentAmount++;

        if (currentAmount > maxScore)
        {
            maxScore = currentAmount;
        }
    }

    public void PlayerDamage()
    { 
        currentLives--;

        if (currentLives >= 0) 
        {
            Image image = images[currentLives];

            image.sprite = damageSprite;
            image.transform.DOShakeScale(0.5f, 0.3f);
        }

        if (currentLives <= 0)
        {
            gameOverEvent.sentInt = maxScore;
            gameOverEvent.Raise();            
        }
    }

    public void PlayerHealth()
    {
        currentLives++;

        if (currentLives >= lives)
        {
            currentLives = lives;
        }

        for (int i = 0; i < currentLives; i++)
        {
            images[i].sprite = normalSprite;
        }       
    }

    public int GetPlayerScore() 
    {
        return currentAmount;
    }
}
