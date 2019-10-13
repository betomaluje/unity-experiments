using UnityEngine;

/**
 * Common interface for every skill
 */ 
public interface SkillBehaviour
{
    void performSkill(int direction);

    void setPosition(Vector3 position);
}
