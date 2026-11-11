using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

public class Shop : MonoBehaviour
{
    public GameObject purchaseMessage;
    bool isinRange;
    public bool isRestoreHealth, isWeaponSwap, isHealthUpgrade;
    public int itemPrice;
    public int healthUpgradeAmount = 1;
    public TextMeshProUGUI info;
    public SpriteRenderer gunsprite;
    public GunManager[] gunList;
    private GunManager gun;

    void Start()
    {
        if (isWeaponSwap)
        {
            gun = gunList[Random.Range(0, gunList.Length)];
            info.text = "Buy " + gun.weaponName + "\n- " + gun.cost + " Coins -";
            gunsprite.sprite = gun.sprite;
            itemPrice = gun.cost;
        }

    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            purchaseMessage.SetActive(true);
            isinRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            purchaseMessage.SetActive(false);
            isinRange = false;
        }
    }

    public void OnBuyItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isinRange)
            {
                if (PlayerMoney.instance.currentMoney >= itemPrice)
                {
                    PlayerMoney.instance.TryRemoveMoney(itemPrice);
                    if (isRestoreHealth)
                    {
                        HealthController.instance.HealPlayer(1);
                    }
                    else if (isWeaponSwap)
                    {
                        PlayerManager.instance.SwitchPlayerGun(gunList.ToList().IndexOf(gun));
                    }
                    else if (isHealthUpgrade)
                    {
                        HealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }

                    gameObject.SetActive(false);
                    isinRange = false;
                }
            }
        }
    }
}
