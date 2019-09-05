using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public float damage;

    public void perform() {
        Debug.Log(skillName + " performed");
    }
}
