using System.Collections;
using UnityEngine;

/**
 * Class in charge of adding, removing, checking and call the perform method of each skill
 */ 
public class SkillsManager : MonoBehaviour
{
    [SerializeField] private Transform shootingPosition;

    private Skill skill;

    public void AddSkill(Skill newSkill)
    {
        AddSkill(newSkill, gameObject);
    }

    public void AddSkill(Skill newSkill, GameObject targetObject)
    {
        skill = newSkill;
        Debug.Log(skill + " added");

        // now we need to setup the new skill
        skill.initSkill(targetObject);
        skill.PlayParticles(targetObject.transform);
    }

    public void RemoveSkill()
    {
        if (skill != null)
        {
            Debug.Log(skill + " removed");
            skill = null;
        }
    }

    public void DoSkill(int direction)
    {
        if (skill != null)
        {
            Debug.Log("do skill " + skill);
            skill.setPosition(shootingPosition.position);
            skill.performSkill(direction);
            StartCoroutine(skill.performCorroutineSkill(direction));
        }
    }

    public bool hasSkill()
    {
        return skill != null;
    }
}
