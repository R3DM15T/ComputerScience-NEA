using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum EnemyStates
{
    Attack,
    Follow,
    Patrol
}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] private float cellSize = 10f;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector3 lastPosition; 
    Rigidbody2D rigidBody;
    public EnemyStates currentState;
    public Transform player;
    public float distance;
    public List<Node> path;

    public Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Start()
    {
        GenerateGrid();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.position);
        FiniteStates();
        FlipSprite();
    }

    void GenerateGrid()
    {
        int width = 50;
        int height = 50;

        for (int x = -width; x < width; x++)
        {
            for (int y = -height; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Vector3 worldPos = GridToWorld(pos);
                bool blocked = Physics2D.OverlapCircle(worldPos, cellSize * 0.4f, platformLayer);
                bool isWalkable = !blocked;

                grid[pos] = new Node(pos, isWalkable);
            }
        }
    }

    void FiniteStates()
    {
        switch (currentState) //switch case statement for the different enemy states
        {
            case EnemyStates.Attack:
                if (distance > attackRange)
                    currentState = EnemyStates.Follow; //enemy follows player if distance is more than the attack range
                break;
            case EnemyStates.Follow:
                FollowPlayer(); //uses A* pathfinding to follow player
                if (distance < attackRange)
                    currentState = EnemyStates.Attack; //enemy attacks player if the distance is less than attack range
                break;
            case EnemyStates.Patrol:
                if (distance < detectRange)
                    currentState = EnemyStates.Follow; //enemy follows player is distance is less than the detection range
                break;
        }
    }



    void FollowPlayer()
    {
        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int end = WorldToGrid(player.position);
        path = PathFind(start, end);

        if (path != null && path.Count > 1)
        {
            Vector3 nextPos = GridToWorld(path[1].gridPos);
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            transform.localScale = new Vector2(1f * Mathf.Sign(rigidBody.linearVelocity.x), 1f);
        }
    }

    public List<Node> PathFind(Vector2Int start, Vector2Int end)
    {
        Node startNode = grid[start];
        Node endNode = grid[end];
        endNode.isWalkable = true; //just to force the target to be walkable, too many bugs without this

        List<Node> openSet = new List<Node>(); //list of the nodes it needs to check
        HashSet<Node> closeSet = new HashSet<Node>(); //the nodes that are already checked

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) //for loop to find the node with the lowest f cost so shortest path
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode); //moving the current node from open to closed as it has been processed
            closeSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {

                if (!neighbour.isWalkable || closeSet.Contains(neighbour))
                    continue;

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);


                if (newCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }


        }
        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }


    public List<Node> GetNeighbours(Node node) //get all the neibouring nodes 
    {
        List<Node> neighbours = new List<Node>();

        Vector2Int[] directions =
        {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPosition = node.gridPos + dir;

            if (grid.ContainsKey(checkPosition))
            {
                neighbours.Add(grid[checkPosition]);
            }
        }

        return neighbours;
    }

    int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridPos.x - b.gridPos.x);
        int dy = Mathf.Abs(a.gridPos.y - b.gridPos.y);

        return dx + dy;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.y / cellSize);

        return new Vector2Int(x, y);
    }


    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float x = gridPos.x * cellSize;
        float y = gridPos.y * cellSize;

        return new Vector3(x, y, 0);
    }


    void FlipSprite()
    {
        float movement = transform.position.x - lastPosition.x;

        if (movement > 0.01f) // moving to the right
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement < -0.01f) // moving to the left
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        lastPosition = transform.position;
    }




    void OnDrawGizmos()
    {
        if (path == null) return;

        Gizmos.color = Color.red;

        foreach (var node in grid.Values)
        {
            Vector3 worldPos = GridToWorld(node.gridPos);

            if (node.isWalkable)
            {
                Gizmos.color = Color.blue;//walkable cells
            }
            else
            {
                Gizmos.color = Color.red; //blocked cells
            }

            Gizmos.DrawCube(worldPos, Vector3.one * 0.3f);
        }

        foreach(Node node in path)
        {
            Vector3 worldPos = GridToWorld(node.gridPos);
            Gizmos.color = Color.green; //path
            Gizmos.DrawCube(worldPos, Vector3.one * 0.3f);
        }



    }
}



public class Node
{
    public int gCost; //the distance from the node to the start node
    public int hCost; //the distance from the node to the end node
    public int fCost => gCost + hCost; //overall distance from start to end node
    public Node parent;
    public bool isWalkable;
    public Vector2Int gridPos;
    public Node(Vector2Int pos, bool walkable)
    {
        gridPos = pos;
        isWalkable = walkable;
        gCost = int.MaxValue; 
    }
}
