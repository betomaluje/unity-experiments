using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Body Changer Skill")]
public class BodyChangerSkill : Skill
{
    public float scaleFactor;
    public float timeForPower;

    private GameObject targetObject;

    public override void initSkill(GameObject skillObject)
    {
        targetObject = skillObject;
    }

    public override void performSkill(int direction)
    {        
    }

    public override IEnumerator performCorroutineSkill(int direction)
    {
        targetObject.transform.DOScale(scaleFactor, 0.5f);
        yield return new WaitForSeconds(timeForPower);
        targetObject.transform.DOScale(1, 0.5f);
    }    

    public override void setPosition(Vector3 position)
    {
        
    }
}
