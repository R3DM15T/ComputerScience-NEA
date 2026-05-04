using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public class ChunkGeneration : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] GameObject startRoomPrefab;
    [SerializeField] int spawnRoomDelay;
    [SerializeField] int numberOfRooms = 10;
    [SerializeField] float roomxSize = 20f;
    [SerializeField] float roomySize = 20f;

    private Dictionary<Vector2Int, RoomController> placedRooms = new Dictionary<Vector2Int, RoomController>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    void Start()
    {
        SpawnStartRoom();
    }


    void SpawnStartRoom()
    {
        Vector2Int startPosition = Vector2Int.zero;
        SpawnRoom(startPosition, startRoomPrefab);
        roomQueue.Enqueue(startPosition); //Add the start position to the queue

        Debug.Log($" The start room spawned at {startPosition}");
        StartCoroutine(GenerateChunk());


    }

    private IEnumerator GenerateChunk()
    {
        while (roomQueue.Count > 0 && placedRooms.Count < numberOfRooms) //while there is rooms to be processed and the max rooms has not been reached
        {
            Vector2Int currentPosition = roomQueue.Dequeue(); //get the next room position
            RoomController currentRoom = placedRooms[currentPosition];//get room at that set position

            int remainingRooms = numberOfRooms - placedRooms.Count;//find remaning amount of rooms to spawn in 

            List<Direction> availableDirections = new List<Direction>(currentRoom.availableDoors);
            RandomiseList(availableDirections);
            int activeBranches = roomQueue.Count;

            foreach (Direction dir in availableDirections) //loop through each door direction in the room
            {
                Vector2Int newPosition = currentPosition + DirectionToVector(dir);
                Debug.Log($"Spawning in room at {newPosition} with {currentRoom.availableDoors.Count} doors");

                if (placedRooms.ContainsKey(newPosition))//this is to skip if a room already exists at this position to prevent the rooms from overlapping
                {
                    Debug.Log($"Position {newPosition} already has a room, skipping room");
                    continue;
                }
                int roomsLeftForThisBranch = remainingRooms - activeBranches; //calculatye how many rooms left for this branch


                GameObject roomPrefab;
                if (roomsLeftForThisBranch <=1) //if branch can only hold one more room then make it a dead end so no unused doors
                {
                    // last room - force a dead end (only the connecting door)
                    roomPrefab = FindRoomWithOneDoor(Opposite(dir));
                    Debug.Log($"Branch ending at {newPosition} - only {roomsLeftForThisBranch} rooms left for this branch");
                    // if no ded end room exists, fall back to normal
                    if (roomPrefab == null)
                    {
                        roomPrefab = FindRoomWithDoor(Opposite(dir));
                    }
                }
                else
                {
                    roomPrefab = FindRoomWithDoor(Opposite(dir));
                }
                if (roomPrefab != null)
                {
                    SpawnRoom(newPosition, roomPrefab);
                    roomQueue.Enqueue(newPosition);
                }
                else
                {
                    Debug.LogError($"No suitable room found for direction {Opposite(dir)}");
                }


            }
            yield return new WaitForSeconds(spawnRoomDelay); //spawn next rooms after set delay (needed for bug testing)
        }

    }

    void SpawnRoom(Vector2Int gridPosition, GameObject prefab)
    {
        Vector3 gamePosition = new Vector3(gridPosition.x * roomxSize, gridPosition.y * roomySize, -2); //converting the grid position into the actual game position 
        GameObject roomObject = Instantiate(prefab, gamePosition, Quaternion.identity); //Instantiating the room into the game scene
        RoomController room = roomObject.GetComponent<RoomController>();
        placedRooms.Add(gridPosition, room); // Storing the room in the dictionary with its grid position
    }

    GameObject FindRoomWithDoor(Direction requiredDoorDirection) //finding a room prefab that has a door direction that is needed
    {
        List<GameObject> suitableRooms = new List<GameObject>();
        foreach(GameObject room in roomPrefabs)
        {
            RoomController r = room.GetComponent<RoomController>();

            if(r.availableDoors.Contains(requiredDoorDirection))
            {
                suitableRooms.Add(room);
            }
        }
        return suitableRooms[Random.Range(0, suitableRooms.Count)];
    }

    GameObject FindRoomWithOneDoor(Direction requiredDoorDirection)
    {
        List<GameObject> suitableRooms = new List<GameObject>();

        foreach (GameObject room in roomPrefabs)
        {
            RoomController r = room.GetComponent<RoomController>();

            //has to only have one door and the one required
            if (r.availableDoors.Contains(requiredDoorDirection) && r.availableDoors.Count == 1)
            {
                suitableRooms.Add(room);
            }
        }

        if (suitableRooms.Count > 0)
            return suitableRooms[Random.Range(0, suitableRooms.Count)];

        return null; //no ded room found
    }

    Vector2Int DirectionToVector(Direction dir)
    {
        switch (dir) //switch case to convert the direction into a grid movement vector
        {
            case Direction.Up: return Vector2Int.up;
            case Direction.Down: return Vector2Int.down;
            case Direction.Left: return Vector2Int.left;
            case Direction.Right: return Vector2Int.right;
        }
        return Vector2Int.zero; 
    }


    Direction Opposite(Direction dir)// returning the oppositive direction to make the doors in each room connect to one another
    {
        switch (dir)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        return dir; 
    }

    void RandomiseList<T>(List<T> list)
    {
        //select random in list
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
