using UnityEngine;

public class SkillsManager : MonoBehaviour, SkillBehaviour
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

    public void performSkill()
    {
        if (skill != null)
        {
            skill.perform();
        }
    }

    public bool hasSkill()
    {
        return skill != null;
    }
}
