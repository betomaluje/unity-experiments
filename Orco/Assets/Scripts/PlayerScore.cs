using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TMP_Text mText;
    private int currentAmount = 0;

    private void Update()
    {
        mText.text = currentAmount.ToString();
    }

    public void Reset()
    {
        currentAmount = 0;
    }

    public void Add(GameObject go)
    {
        currentAmount++;
    }

    public void Substract(GameObject go)
    {
        currentAmount--;
    }
}
