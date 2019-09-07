using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Spawn Object Skill")]
public class SpawnObjectSkill : Skill
{
    public float distanceToTeleport;
    public bool isInmediate = false;

    public GameObject targetObject;

    private Vector3 parentObjectPosition;

    public override void initSkill(GameObject skillObject)
    {
    }

    public override void performSkill(int direction)
    {
        Vector2 pos = parentObjectPosition;
        pos.x = pos.x + distanceToTeleport * direction;

        Instantiate(targetObject, pos, Quaternion.identity);
    }

    public override IEnumerator performCorroutineSkill(int direction)
    {
        yield return null;
    }

    public override void setPosition(Vector3 position)
    {
        parentObjectPosition = position;
    }
}
