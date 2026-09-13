using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{

    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform gunPos;
    [SerializeField] float shotCooldown;
    [SerializeField] float shotCounter;
    private bool isFiring;
    public string weaponName;
    public Sprite weaponImageUI;
    

    void Start()
    {
        
    }

    void Update()
    {
        isFiring = Mouse.current.leftButton.isPressed;
        if (shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }

        if (isFiring && shotCounter <= 0f)
        {
            FireBullet();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFiring = true;
            shotCounter = shotCooldown;
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
