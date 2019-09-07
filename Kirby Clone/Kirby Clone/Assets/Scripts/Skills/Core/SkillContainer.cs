using UnityEngine;

/**
* Class that absorbable objects have to contain their skill
 */
public class SkillContainer : MonoBehaviour
{
    public Skill skill;

    public Skill getSkill()
    {
        return skill;
    }

}
