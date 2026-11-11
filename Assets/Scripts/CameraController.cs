using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float moveSpeed = 1f;
    public Transform movePoint;
    public GameObject miniMap;
    public Camera mainCam, expandedMapCam;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }


    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(movePoint.position.x + 1, movePoint.position.y + 1, -10), moveSpeed * Time.deltaTime);
    }

    public void ChangeTarget(Transform newTarget)
    {
        movePoint = newTarget;
    }


    public void TurnOnExpandedMap()
    {
        expandedMapCam.enabled = true;
        mainCam.enabled = false;
        miniMap.SetActive(false);
    }

    public void TurnOffExpandedMap()
    {
        expandedMapCam.enabled = false;
        mainCam.enabled = true;
        miniMap.SetActive(true);
    }
}
