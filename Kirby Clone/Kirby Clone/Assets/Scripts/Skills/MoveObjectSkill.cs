using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Move Object Skill")]
public class MoveObjectSkill : Skill
{
    public float distanceToTeleport;
    public float timeForSkill;

    private GameObject targetObject;

    public override void initSkill(GameObject skillObject)
    {
        targetObject = skillObject;
    }

    public override void performSkill(int direction)
    {
        Vector2 pos = targetObject.transform.position;
        pos.x = pos.x + distanceToTeleport * direction;        

        targetObject.transform.DOMove(pos, timeForSkill, false);
    }

    public override IEnumerator performCorroutineSkill(int direction)
    {
        yield return null;
    }

    public override void setPosition(Vector3 position)
    {

    }
}
