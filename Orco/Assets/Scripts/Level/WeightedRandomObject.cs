using System.Collections.Generic;
using UnityEngine;

public class WeightedRandomObject : MonoBehaviour
{
    [SerializeField] private List<WeightedObject> toSpawn;

    private Dictionary<GameObject, int> myDictionary;
    
    void Start()
    {
        myDictionary = new Dictionary<GameObject, int>();
        foreach (var entry in toSpawn)
        {
            myDictionary.Add(entry.key, entry.value);
        }

        GameObject objectToSpawn = WeightedRandomizer.From(myDictionary).TakeOne();

        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }    
}

[System.Serializable]
public class WeightedObject
{
    public GameObject key;
    public int value;
}