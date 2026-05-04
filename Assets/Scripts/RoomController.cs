using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomController : MonoBehaviour
{
    public bool doorClose;
    public GameObject doors;
    public GameObject roomMapLayout;
    public List<Direction> availableDoors;
    public List<GameObject> enemies = new List<GameObject>();
    public bool roomActive;
    public bool closeDoorOnWhenEntered, openDoorOnEnemiesCleared;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        EnemyCheck();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);
            if (closeDoorOnWhenEntered)
            {
                doors.SetActive(true);
                roomMapLayout.SetActive(true);
            }

        }

    }

    public void EnemyCheck()
    {
        if (enemies.Count > 0 && openDoorOnEnemiesCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            if (enemies.Count == 0)
            {
                doors.SetActive(false);
                closeDoorOnWhenEntered = false;
            }
        }
    }

}
