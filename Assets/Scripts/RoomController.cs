using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomController : MonoBehaviour
{
    public bool doorClose;
    public GameObject doors;
    public List<Direction> availableDoors;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);

            if (doorClose)
            {
                StartCoroutine(closeDelay());
                StartCoroutine(openDoor());
            }
        }

    }
    
    public IEnumerator closeDelay()
    {
        yield return new WaitForSeconds(1);
        doors.SetActive(true);
    }

    public IEnumerator openDoor()
    {
        yield return new WaitForSeconds(5);
        doors.SetActive(false);
    }
}
