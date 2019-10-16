using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfRooms = 10;
    [SerializeField] private int scale;
    [SerializeField] private GameObject[] roomsObjects;
    
    private List<Vector2> directionsList;
    private List<Vector3> roomPositions;
    
    void Start()
    {
        directionsList = new List<Vector2>();
        directionsList.Add(new Vector2(0, 1)); //up
        directionsList.Add(new Vector2(0, -1)); // down
        directionsList.Add(new Vector2(1, 0)); // left
        directionsList.Add(new Vector2(-1, 0)); // right

        GenerateMap();
    }

    private void GenerateMap() {
        roomPositions = new List<Vector3>();

        Vector2 newPosition = directionsList[Random.Range(0, directionsList.Count)] * scale;

        // we instantiate first room
        Instantiate(getRoom(), transform.position, Quaternion.identity);
        transform.position += new Vector3(newPosition.x, newPosition.y, 0);

        print("first " + transform.position);

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            newPosition = directionsList[Random.Range(0, directionsList.Count)] * scale;

            if (!roomPositions.Contains(newPosition)) {
                // we instantiate first room
                Instantiate(getRoom(), transform.position, Quaternion.identity);
                transform.position += new Vector3(newPosition.x, newPosition.y, 0);

                roomPositions.Add(transform.position);

                print("room " + i + " -> " + transform.position);
            } else {
                i--;
            }       
        }
    }

    private GameObject getRoom() {
        return roomsObjects[Random.Range(0, roomsObjects.Length)];
    }

    private int RandomExcept(int min, int max, int except)
    {
        int random = Random.Range(min, max);
        if (random >= except) random = (random + 1) % max;
        return random;
    }

    private Vector3 RandomExceptList(int fromNr, int exclusiveToNr, List<Vector3> exceptNr)
    {
        Vector3 randomNr = exceptNr[0];

        while (exceptNr.Contains(randomNr))
        {
            randomNr = roomPositions[Random.Range(fromNr, exclusiveToNr)];
        }

        return randomNr;
    }

}
