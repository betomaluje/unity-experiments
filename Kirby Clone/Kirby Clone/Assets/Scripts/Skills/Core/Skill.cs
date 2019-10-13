using System.Collections;
using UnityEngine;

public abstract class Skill : ScriptableObject, SkillBehaviour
{
    public string skillName;
    public float damage;
    public GameObject absorbParticles;

    public void PlayParticles(Transform t)
    {
        if (absorbParticles != null)
        {
            Instantiate(absorbParticles, t.position, absorbParticles.transform.rotation);
        }
    }

    public abstract void initSkill(GameObject skillObject);

    public abstract void setPosition(Vector3 position);

    public abstract void performSkill(int direction);

    public abstract IEnumerator performCorroutineSkill(int direction);
}
