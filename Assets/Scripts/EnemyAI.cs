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

    public EnemyStates currentState;
    public Transform player;
    public float distance;

    public Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    void Start()
    {

    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.position);
        FiniteStates();
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
    }

    public List<Node> PathFind(Vector2Int start, Vector2Int end)
    {
        Node startNode = grid[start];
        Node endNode = grid[end];

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


        }
        return null;
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
}
