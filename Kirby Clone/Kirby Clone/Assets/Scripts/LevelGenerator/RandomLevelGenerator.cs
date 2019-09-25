using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomLevelGenerator : MonoBehaviour
{
    private enum RoomEnum
    {                       // Binary   // Decimal
        None = 0,           // 000000   0
        Left = 1 << 0,      // 000001   1
        Right = 1 << 1,     // 000010   2
        Bottom = 1 << 2,    // 000100   4
        Top = 1 << 3        // 001000   8
    }

    public bool shouldBeginInmediatly = false;

    public Transform[] startingPositions;

    public Transform[] allPositions;

    /**
     * Normal Rooms direction indexes
     * 0 -> Closed Room
     * 1 -> LeftRight (LR)
     * 2 -> LeftRightBottom (LRB)
     * 3 -> LeftRightTop (LRT)
     * 4 -> LeftRightBottomTop (LRBT)
     */
    public GameObject[] rooms;

    /**
     * Edge Rooms indexes
     * 0 -> only Left (L)
     * 1 -> only Left and Bottom (LB)
     * 2 -> only Left, Bottom and Top (LBT)
     * 3 -> only Left and Top (LT)
     * 4 -> only Right (R)
     * 5 -> only Right and Bottom (RB)
     * 6 -> only Right, Bottom and Top (RBT)
     * 7 -> only Right and Top (RT)
     */
    public GameObject[] edgeRooms;

    private int direction;

    private bool stopPathGeneration;
    private int downCounter;

    private float moveIncrement;
    private float timeBtwSpawn;
    public float startTimeBtwSpawn;

    public LayerMask whatIsRoom;
    public LayerMask whatIsBorder;

    public float collisionRadius;
    private float minX, maxX, minY;

    public GameEvent firstRoomReady;

    private Vector3 firstRoomPosition;

    private Hashtable playerPath;

    private void Start()
    {
        playerPath = new Hashtable();

        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        AddRoom(rooms[1]);

        firstRoomPosition = transform.position;

        stopPathGeneration = !shouldBeginInmediatly;
        moveIncrement = Mathf.Abs(startingPositions[0].position.x - startingPositions[1].position.x);        

        FindHighestX();
        FindLowestY();
        FindLowestX();

        direction = Random.Range(1, 6);
    }

    private void FindHighestX()
    {
        float highestFillingPosition = -99999f;
        for (int f = 0; f < startingPositions.Length; f++)
        {
            float value = startingPositions[f].position.x;
            if (value > highestFillingPosition)
            {
                highestFillingPosition = value;
                maxX = highestFillingPosition;
            }
        }
    }

    private void FindLowestX()
    {
        float lowestFillingPosition = 99999f;
        for (int f = 0; f < startingPositions.Length; f++)
        {
            float value = startingPositions[f].position.x;
            if (value < lowestFillingPosition)
            {
                lowestFillingPosition = value;
                minX = lowestFillingPosition;
            }
        }
    }

    private void FindLowestY()
    {
        float lowestFillingPosition = 99999f;
        for (int f = 0; f < allPositions.Length; f++)
        {
            float value = allPositions[f].position.y;
            if (value < lowestFillingPosition)
            {
                lowestFillingPosition = value;
                minY = lowestFillingPosition;
            }
        }
    }

    public void StartGeneratingRoom() {
        stopPathGeneration = false;
    }

    public void Reset() {
        StartCoroutine(ResetMap());    
    }

    private IEnumerator ResetMap() {
        yield return new WaitForSeconds(1f);

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

        foreach (var go in rooms)
        {
            Destroy(go);
            yield return new WaitForSeconds(0.1f);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var go in enemies)
        {
            Destroy(go);
            yield return new WaitForSeconds(0.1f);
        }
        

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            Destroy(player);
        }

        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        AddRoom(rooms[1]);

        firstRoomPosition = transform.position;
        stopPathGeneration = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!stopPathGeneration)
        {
            if (timeBtwSpawn <= 0)
            {
                Move();
                timeBtwSpawn = startTimeBtwSpawn;
            }
            else
            {
                timeBtwSpawn -= Time.deltaTime;
            }
        }
    }

    /**
     * Given the current direction we need to look to it's edge to see if we are ath the edge of the map or not. If we are in an edge
     * we would need to replace the room so the player can't escape from the maze
     * 
     */
    private void CheckBorder(GameObject previousRoom)
    {
        RoomEnum roomType = RoomEnum.None;

        RaycastHit2D hiDown = Physics2D.Raycast(previousRoom.transform.position, previousRoom.transform.up * -1, collisionRadius, whatIsBorder);
        RaycastHit2D hiUp = Physics2D.Raycast(previousRoom.transform.position, previousRoom.transform.up, collisionRadius, whatIsBorder);
        RaycastHit2D hiLeft = Physics2D.Raycast(previousRoom.transform.position, previousRoom.transform.right * -1, collisionRadius, whatIsBorder);
        RaycastHit2D hiRight = Physics2D.Raycast(previousRoom.transform.position, previousRoom.transform.right, collisionRadius, whatIsBorder);

        if (hiLeft)
        {
            roomType |= RoomEnum.Left; // 1
        }

        if (hiRight)
        {
            roomType |= RoomEnum.Right; // 2
        }

        if (hiDown)
        {
            roomType |= RoomEnum.Bottom; // 4
        }

        if (hiUp)
        {
            roomType |= RoomEnum.Top; // 8
        }

        int pos = -1;

        switch (roomType)
        {
            case RoomEnum.Left:
                //Debug.Log("Need to close left");
                pos = 0;
                break;
            case RoomEnum.Right:
                //Debug.Log("Need to close right");
                pos = 3;
                break;
            case RoomEnum.Bottom:
                //Debug.Log("Need to close bottom");
                pos = 6;
                break;
            case RoomEnum.Top:
                //Debug.Log("Need to close top");
                pos = 7;
                break;
            case RoomEnum.Left | RoomEnum.Top:
                //Debug.Log("Need to close left and top");
                pos = 2;
                break;
            case RoomEnum.Left | RoomEnum.Bottom:
                //Debug.Log("Need to close left and bottom");
                pos = 1;
                break;
            case RoomEnum.Right | RoomEnum.Top:
                //Debug.Log("Need to close right and top");
                pos = 5;
                break;
            case RoomEnum.Right | RoomEnum.Bottom:
                //Debug.Log("Need to close right and bottom");
                pos = 4;
                break;
            default:
                break;
        }

        if (pos != -1)
        {
            // we overlay a wall
            Instantiate(edgeRooms[pos], transform.position, Quaternion.identity);
        }
    }

    private void CheckBorders()
    {
        foreach (Transform pointTransform in allPositions)
        {
            transform.position = pointTransform.position;
            CheckBorder(pointTransform.gameObject);
        }

        //GenerateLockAndKey();    

        firstRoomReady.sentVector3 = firstRoomPosition;
        firstRoomReady.Raise();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * -1 * collisionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * collisionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * -1 * collisionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * collisionRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }

    private void Move()
    {
        if (direction == 1 || direction == 2)
        {
            // MOVE RIGHT      

            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 pos = new Vector2(transform.position.x + moveIncrement, transform.position.y);
                transform.position = pos;

                // all rooms have left and right so we don't care
                int randRoom = Random.Range(1, 4);
                AddRoom(rooms[randRoom]);

                // we make sure it doesn't move left now (avoiding overlaps)
                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 1;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                // since we are in the edge, we move down
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)
        {
            // MOVE LEFT

            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 pos = new Vector2(transform.position.x - moveIncrement, transform.position.y);
                transform.position = pos;

                // all rooms have left and right so we don't care
                int randRoom = Random.Range(1, 4);
                AddRoom(rooms[randRoom]);

                // we make sure it doesn't move right now (avoiding overlaps)
                direction = Random.Range(3, 6);
            }
            else
            {
                // since we are in the edge, we move down
                direction = 5;
            }
        }
        else if (direction == 5)
        {
            // MOVE DOWN

            downCounter++;
            if (transform.position.y > minY)
            {
                // Now I must replace the room BEFORE going down with a room that has a DOWN opening, so type 3 or 5
                Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, collisionRadius, whatIsRoom);

                if (previousRoom != null) 
                {
                    RoomType roomType = previousRoom.GetComponentInParent<RoomType>();

                    if (roomType.type != 4 && roomType.type != 2)
                    {
                        // My problem : if the level generation goes down TWICE in a row, there's a chance that the previous room is just 
                        // a LRB, meaning there's no TOP opening for the other room !                     
                        if (downCounter >= 2)
                        {
                            // if we move 2 or more times down, we need to spawn a room with all openings                        
                            AddRoom(rooms[4]);
                        }
                        else
                        {
                            // if top room doesn't have bottom opening                        
                            int randRoomDownOpening = Random.Range(2, 5);
                            if (randRoomDownOpening == 3)
                            {
                                randRoomDownOpening = 2;
                            }

                            Instantiate(rooms[randRoomDownOpening], previousRoom.transform.position, Quaternion.identity);                                            
                        }
                        Destroy(previousRoom.gameObject);
                    }
                }

                Vector2 pos = new Vector2(transform.position.x, transform.position.y - moveIncrement);
                transform.position = pos;

                // Makes sure the room we drop into has a TOP opening !
                int randRoom = Random.Range(3, 5);
                AddRoom(rooms[randRoom]);

                direction = Random.Range(1, 6);
            }
            else
            {
                //Instantiate(theLock, transform.position, Quaternion.identity);
                stopPathGeneration = true;
                StartCoroutine(GenerateOtherRooms());
            }
        }
    }

    private void AddRoom(GameObject room)
    {
        if (!playerPath.ContainsKey(transform.position))
        {
            GameObject r = Instantiate(room, transform.position, Quaternion.identity);
            playerPath.Add(transform.position, r);
        }
    }

    IEnumerator GenerateOtherRooms()
    {
        foreach (Transform pointTransform in allPositions)
        {
            transform.position = pointTransform.position;

            //Collider2D collidingRoom = Physics2D.OverlapCircle(transform.position, collisionRadius, whatIsRoom);
            if (!playerPath.ContainsKey(transform.position))
            {
                // we need to spawn a random room here!
                int randRoom = Random.Range(0, rooms.Length);
                AddRoom(rooms[randRoom]);
                yield return new WaitForSeconds(timeBtwSpawn);
            }            
        }        

        CheckBorders();       
    }

    private void GenerateLockAndKey()
    {
        int lockPos = Random.Range(0, allPositions.Length);
        int keyPos = RandomExcept(0, allPositions.Length, lockPos);

        //Instantiate(theLock, allPositions[lockPos].position, Quaternion.identity);
        //Instantiate(theKey, allPositions[keyPos].position, Quaternion.identity);

        transform.position = Vector3.zero;

        firstRoomReady.sentVector3 = firstRoomPosition;
        firstRoomReady.Raise();
    }

    private int RandomExcept(int min, int max, int except)
    {
        int random = Random.Range(min, max);
        if (random >= except) random = (random + 1) % max;
        return random;
    }

    private int RandomExceptList(int fromNr, int exclusiveToNr, List<int> exceptNr)
    {
        int randomNr = exceptNr[0];

        while (exceptNr.Contains(randomNr))
        {
            randomNr = Random.Range(fromNr, exclusiveToNr);
        }

        return randomNr;
    }
}
