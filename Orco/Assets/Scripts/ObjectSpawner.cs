using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public float spawnTime = 5f;
    public int maxItemsAtATime = 3;
    
    private int currentItems = 0;
    private Camera mCamera;

    private void Awake()
    {
        mCamera = Camera.main;           

        StartCoroutine(RespawnItem());
    }

    private void Spawn()
    {
        if (currentItems >= maxItemsAtATime)
        {
            return;
        }

        currentItems++;

        int selectedObject = Random.Range(0, objects.Length);

        float randomX = Random.Range(0, Screen.width);
        float randomY = Random.Range(0, Screen.height);

        Vector3 spawnPoint = mCamera.ScreenToWorldPoint(new Vector3(randomX, randomY, mCamera.farClipPlane / 2));        

        GameObject weapon = objects[selectedObject];

        Instantiate(weapon, spawnPoint, Quaternion.identity);

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
