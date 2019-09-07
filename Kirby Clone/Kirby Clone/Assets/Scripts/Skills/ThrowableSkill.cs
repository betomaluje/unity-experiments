using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Throwable Skill")]
public class ThrowableSkill : Skill
{
    public GameObject throwableObject;
    public float speed;
    private Vector3 shootingPosition;

    public override void initSkill(GameObject skillObject)
    {
        
    }

    public override void setPosition(Vector3 position)
    {
        shootingPosition = position;
    }

    public override void performSkill(int direction)
    {
        Rigidbody2D rb = Instantiate(throwableObject, shootingPosition, Quaternion.identity).GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(new Vector2(speed * direction, 0), ForceMode2D.Impulse);
            Destroy(rb.gameObject, 1f);
        }
    }

    public override IEnumerator performCorroutineSkill(int direction)
    {
        yield return null;
    }
}
