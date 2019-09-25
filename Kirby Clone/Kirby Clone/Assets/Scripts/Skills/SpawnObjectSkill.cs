using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Spawn Object Skill")]
public class SpawnObjectSkill : Skill, ImplosionListener
{
    public float distanceToTeleport;
    public bool isInmediate = false;

    public GameObject[] targetObjects;
    public GameObject targetParticles;

    private Vector3 parentObjectPosition;
    private Vector2 targetPosition;

    public override void initSkill(GameObject skillObject)
    {
    }

    public override void performSkill(int direction)
    {       
        targetPosition = parentObjectPosition;
        targetPosition.x = targetPosition.x + distanceToTeleport * direction;

        ImplosionEffect implosionEffect = Instantiate(targetParticles, targetPosition, Quaternion.identity).GetComponent<ImplosionEffect>();
        
        if (implosionEffect != null)
        {
            implosionEffect.callback = this;
        }

        SoundManager.instance.Play("Wall_1");
    }

    public override IEnumerator performCorroutineSkill(int direction)
    {
        yield return null;
    }

    public override void setPosition(Vector3 position)
    {
        parentObjectPosition = position;
    }

    public void onStartImplosion()
    {
        
    }

    public void onFinishImplosion()
    {
        foreach (var targetObject in targetObjects)
        {
            Instantiate(targetObject, targetPosition, Quaternion.identity);
            SoundManager.instance.Play("Wall_2");
        }        
    }
}
