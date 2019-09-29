using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform parentTransform;
    public GameObject[] objects;
    public float spawnTime = 5f;
    public int maxItemsAtATime = 3;

    private List<Vector3> centerPoints;
    private int currentItems = 0;

    private void Awake()
    {
        centerPoints = new List<Vector3>(parentTransform.childCount);

        foreach (Transform t in parentTransform)
        {
            // we loop for each child game object on the parentTransform GameObject            
            Vector3 newPosition = t.position;

            Renderer spriteRenderer = t.gameObject.GetComponent<Renderer>();
            if (spriteRenderer != null)
            {
                newPosition.y = t.position.y + (spriteRenderer.bounds.size.y / 2) + 1f;
            }
            else
            {
                newPosition.y = t.position.y + 1;
            }

            centerPoints.Add(newPosition);
        }

        StartCoroutine(RespawnItem());
    }

    private void Spawn()
    {
        if (currentItems >= maxItemsAtATime)
        {
            Debug.LogWarning("Maximum capacity reached!");
            return;
        }

        currentItems++;

        int selectedWeapon = Random.Range(0, objects.Length);
        int spawnPointIndex = Random.Range(0, centerPoints.Count);

        GameObject weapon = objects[selectedWeapon];

        Instantiate(weapon, centerPoints[spawnPointIndex], Quaternion.identity);

        StartCoroutine(RespawnItem());
    }

    public void ResetObject()
    {
        currentItems--;

        // now we try to spawn another one
        StartCoroutine(RespawnItem());
    }

    public IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(spawnTime);
        Spawn();
    }
}
