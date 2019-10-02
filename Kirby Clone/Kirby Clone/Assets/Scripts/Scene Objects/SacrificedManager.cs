using UnityEngine;
using TMPro;

public class SacrificedManager : MonoBehaviour
{
    [SerializeField] private TMP_Text mText;
    private int currentAmount = 0;

    private void Update()
    {
        mText.text = currentAmount.ToString();
    }

    public void AddSacrifice(GameObject go)
    {
        currentAmount++;
    }
}
