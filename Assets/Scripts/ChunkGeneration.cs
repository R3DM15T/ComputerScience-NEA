using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] int numberOfRooms = 10;
    [SerializeField] float roomxSize = 20f;
    [SerializeField] float roomySize = 20f;

    private Dictionary<Vector2Int, RoomController> placedRooms = new Dictionary<Vector2Int, RoomController>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    void Start()
    {
        ChunkGenerate();
    }


    void ChunkGenerate()
    {
        Vector2Int startPosition = Vector2Int.zero;
        SpawnRoom(startPosition, startRoomPrefab);
        roomQueue.Enqueue(startPosition); //Add the start position to the queue
        while (roomQueue.Count > 0 && placedRooms.Count < numberOfRooms) //while there is rooms to be processed and the max rooms has not been reached
        {
            Vector2Int currentPosition = roomQueue.Dequeue(); //get the next room position
            RoomController currentRoom = placedRooms[currentPosition];//get room at that set position

            foreach (Direction dir in currentRoom.availableDoors) //loop through each door direction in the room
            {
                Vector2Int newPosition = currentPosition + DirectionToVector(dir);
                if(placedRooms.ContainsKey(newPosition))//this is to skip if a room already exists at this position to prevent the rooms from overlapping
                {
                    continue;
                }


                GameObject roomPrefab = FindRoomWithDoor(Opposite(dir));
                SpawnRoom(newPosition, roomPrefab);
                roomQueue.Enqueue(newPosition);
            }
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
}
