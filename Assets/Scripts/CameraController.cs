using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float moveSpeed = 1f;
    public Transform movePoint;

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
}
