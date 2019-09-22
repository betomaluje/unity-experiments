using UnityEngine;

public class MachineGunController : MonoBehaviour
{
    private GameObject objectToLookup;

    void Update()
    {
        if (objectToLookup == null)
        {
            return;
        }

        Vector2 direction = new Vector2(
            objectToLookup.transform.position.x - transform.position.x,
            objectToLookup.transform.position.y - transform.position.y);

        transform.right = direction;
    }

    public void SetObjectToLookUp(Vector3 pos)
    {
        objectToLookup = GameObject.FindGameObjectWithTag("Player");
    }
}
