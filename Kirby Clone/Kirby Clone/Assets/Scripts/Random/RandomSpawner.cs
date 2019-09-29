using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private bool createAsChild = false;
    [SerializeField] private List<WeightedObject> toSpawn;
    [SerializeField] private Transform whereToSpawn;

    private Dictionary<GameObject, int> myDictionary;

    // Start is called before the first frame update
    void Start()
    {
        myDictionary = new Dictionary<GameObject, int>();
        foreach (var entry in toSpawn)
        {
            myDictionary.Add(entry.key, entry.value);
        }        

        GameObject objectToSpawn = WeightedRandomizer.From(myDictionary).TakeOne();

        GameObject actualObject = null;
        if (whereToSpawn != null)
        {
            actualObject = Instantiate(objectToSpawn, whereToSpawn.position, Quaternion.identity);
        } else
        {
            actualObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
        
        if (createAsChild)
        {
            actualObject.transform.parent = transform;
        }
    }
}

[System.Serializable]
public class WeightedObject
{
    public GameObject key;
    public int value;
}