using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject, SkillBehaviour
{
    public string skillName;
    public float damage;
  
    public void performSkill()
    {
        Debug.Log(skillName + " performed");
    }
}
