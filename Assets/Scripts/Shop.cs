using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    public GameObject purchaseMessage;
    bool isinRange;
    public bool isRestoreHealth, isWeaponSwap, isHealthUpgrade;
    public int itemPrice;
    public int healthUpgradeAmount = 1;

    void Start()
    {
        
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
                        PlayerManager.instance.SwitchPlayerGun();
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
