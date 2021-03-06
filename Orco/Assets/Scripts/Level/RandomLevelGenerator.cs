﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGenerator : MonoBehaviour
{
    [SerializeField] private float timeBtwRooms = 0;
    [SerializeField] private int numberOfRooms = 10;
    [SerializeField] private float scale;
    [SerializeField] private GameObject[] roomsObjects;
    [SerializeField] private GameObject horizontalBridge;
    [SerializeField] private GameObject verticalBridge;

    [SerializeField] private GameObject winObject;

    private List<Vector2> directionsList;
    private List<Vector3> roomPositions;
    private List<GameObject> addedRooms;
    private List<Bridge> bridges;

    class Bridge
    {
        public enum Type
        {
            HORIZONTAL, VERTICAL
        }

        public Vector3 position;
        public Type type;
        public Transform parent;

        public Bridge(Type t, Vector3 pos, Transform p)
        {
            type = t;
            position = pos;
            parent = p;
        }

    }

    void Start()
    {
        directionsList = new List<Vector2>();
        directionsList.Add(new Vector2(0, 1)); //up
        directionsList.Add(new Vector2(0, -1)); // down
        directionsList.Add(new Vector2(1, 0)); // left
        directionsList.Add(new Vector2(-1, 0)); // right

        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap() {
        roomPositions = new List<Vector3>();
        addedRooms = new List<GameObject>();
        bridges = new List<Bridge>();

        // we instantiate first room
        GameObject firstRoom = Instantiate(getRoom(), transform.position, Quaternion.identity);        
        roomPositions.Add(transform.position);
        addedRooms.Add(firstRoom);

        yield return new WaitForSeconds(timeBtwRooms);

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            Vector2 direction = GetRandomDirection();
            Vector2 temp2DPosition = direction * scale;            
            Vector3 nextPosition = new Vector3(temp2DPosition.x, temp2DPosition.y, 0) + transform.position;

            if (!roomPositions.Contains(nextPosition)) {

                // we instantiate a room
                GameObject room = Instantiate(getRoom(), nextPosition, Quaternion.identity);

                // we need to connect current room with next one                
                Vector2 bridge2DPosition = direction * (scale / 2);
                Vector3 bridgePosition = new Vector3(bridge2DPosition.x, bridge2DPosition.y, 0) + transform.position;
                // when is going down it has 1 square spare on the top.
                // up is fine                                

                if (temp2DPosition.y != 0)
                {
                    bridges.Add(new Bridge(Bridge.Type.VERTICAL, bridgePosition, room.transform));
                } else
                {
                    bridges.Add(new Bridge(Bridge.Type.HORIZONTAL, bridgePosition, room.transform));
                }                                               

                // we update the gameobjects position
                transform.position = nextPosition;

                roomPositions.Add(nextPosition);
                addedRooms.Add(room);

                yield return new WaitForSeconds(timeBtwRooms);
            } else {
                i--;
            }       
        }

        StartCoroutine(PutBridges());

        // now we need to restore the walls
        RestoreWalls();        
    }

    private IEnumerator PutBridges()
    {
        foreach (var bridge in bridges)
        {
            GameObject bridgeGo;
            if (bridge.type.Equals(Bridge.Type.VERTICAL))
            {
                bridgeGo = Instantiate(verticalBridge, bridge.position, Quaternion.identity);
            } else
            {
                bridgeGo = Instantiate(horizontalBridge, bridge.position, Quaternion.identity);
            }

            bridgeGo.transform.parent = bridge.parent;

            yield return new WaitForSeconds(timeBtwRooms);
        }
    }

    public void PutEndObject() 
    {
        Vector3 lastRoom = roomPositions[roomPositions.Count - 1];

        Instantiate(winObject, lastRoom, Quaternion.identity);
    }

    private void RestoreWalls()
    {
        foreach (var room in addedRooms)
        {
            foreach (Transform pt in room.transform)
            {
                if (pt.gameObject.name.Equals("Grid"))
                {
                    foreach (Transform t in pt)
                    {
                        if (t.CompareTag("Optional Wall"))
                        {
                            CompositeCollider2D collider = t.GetComponentInChildren<CompositeCollider2D>();
                            if (collider)
                            {                                
                                collider.isTrigger = false;
                            }
                        }
                    }
                }                             
            }
        }
    }

    private Vector2 GetRandomDirection()
    {
        return directionsList[Random.Range(0, directionsList.Count)];
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
