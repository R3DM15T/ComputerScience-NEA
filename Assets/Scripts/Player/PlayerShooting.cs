using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform gunPos;
    [SerializeField] float shotCooldown;

    private float shotCounter;
    private bool isFiring;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }


    void FixedUpdate()
    {
        AimAtMouse();
        isFiring = Mouse.current.leftButton.isPressed;
        shotCounter -= Time.deltaTime;

        if (isFiring && shotCounter <= 0f)
        {
            FireBullet();
        }
    }

    void AimAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition); //making variables on the position of the mouse and the position of the camera

        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f); //flip player
            gunPos.localScale = new Vector3(-0.4f, -0.4f, 0.4f);

        }
        else
        {
            transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            gunPos.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        }
        //rotate the gun
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg; //rad2deg converts radians to a degrees
        gunPos.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFiring = true;
            shotCounter = 0f;
        }
        else if (context.canceled)
        {
            isFiring = false;
        }
    }


    void FireBullet()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation); //instantiating in the bullet in gun position
        shotCounter = shotCooldown;
    }
}
