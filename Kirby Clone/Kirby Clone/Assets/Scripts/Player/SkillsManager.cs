using UnityEngine;

/**
 * Class in charge of adding, removing, checking and call the perform method of each skill
 */ 
public class SkillsManager : MonoBehaviour
{
    private Skill skill;

    public void AddSkill(Skill newSkill)
    {
        skill = newSkill;
        Debug.Log(skill + " added");

        // now we need to setup the new skill        
    }

    public void RemoveSkill()
    {
        if (skill != null)
        {
            Debug.Log(skill + " removed");
            skill = null;
        }
    }

    public void DoSkill()
    {
        if (skill != null)
        {
            skill.performSkill();
        }
    }

    public bool hasSkill()
    {
        return skill != null;
    }
}
