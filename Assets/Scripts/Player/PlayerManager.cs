using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] float hazardDamage = 5f;

    public float colorDelay;
    public Color damageColor = Color.red;
    public Color startColor = Color.white;
    public Color transColor = Color.white;
    [SerializeField] Transform gunPos;
    public List<GunManager> guns = new List<GunManager>();
    private int currentGunIndex = 0;

    SpriteRenderer rend;
    BoxCollider2D feetCollider;
    private Camera cam;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<SpriteRenderer>();
        feetCollider = GetComponent<BoxCollider2D>();
        GunUI.instance.weaponImage.sprite = guns[currentGunIndex].weaponImageUI;
        GunUI.instance.weaponNameText.text = guns[currentGunIndex].weaponName;
        
    }


    void FixedUpdate()
    {
        Hazards();
        AimAtMouse();
    }


    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(guns.Count > 1)
            {
                currentGunIndex++;
                if(currentGunIndex >= guns.Count)
                {
                    currentGunIndex = 0;
                }
                SwitchPlayerGun(currentGunIndex);
            }

        }
    }

    void Hazards()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            HealthController.instance.DamageTaken(hazardDamage);

        }
    }



    void AimAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition); //making variables on the position of the mouse and the position of the camera

        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f); //flip player
            gunPos.localScale = new Vector3(-1f, -1f, 1f);

        }
        else
        {
            transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            gunPos.localScale = new Vector3(1f, 1f, 1f);

        }
        //rotate the gun
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg; //rad2deg converts radians to a degrees
        gunPos.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void turnRed()
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        rend.color = damageColor;
        yield return new WaitForSeconds(colorDelay);
        rend.color = startColor;
    }

    public void turnTransparent()
    {
        StartCoroutine(FlashTransparent());
    }

    IEnumerator FlashTransparent()
    {
        rend.color = transColor;
        yield return new WaitForSeconds(colorDelay);
        rend.color = startColor;
    }


    public void SwitchPlayerGun(int currentGunIndex)
    {
        foreach(GunManager gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        GunUI.instance.weaponImage.sprite = guns[currentGunIndex].weaponImageUI;
        GunUI.instance.weaponNameText.text = guns[currentGunIndex].weaponName;
        guns[currentGunIndex].gameObject.SetActive(true);
    }

}
