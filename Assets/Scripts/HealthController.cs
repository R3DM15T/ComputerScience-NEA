using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public static HealthController instance;
    public float currentHealth;
    public float maxHealth = 6;
    public GameObject deathScreen;
    public float damageInvLength = 1f;
    private float invCount;

    [Header("Events")]
    public System.Action OnHealthChanged;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(invCount > 0)
        {
            invCount -= Time.deltaTime;
        }
    }

    void Start()
    {
        
        currentHealth = maxHealth;
    }

    public void DamageTaken(float damage)
    {
        if(invCount <= 0)
        {
            currentHealth -= damage;
            invCount = damageInvLength;
            currentHealth = Mathf.Max(0, currentHealth);
            PlayerManager.instance.turnRed();

            OnHealthChanged?.Invoke();

            if (currentHealth <= 0)
            {
                PlayerDied();
            }
        }

    }

    public void HealPlayer(int healAmount = 1)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);

        OnHealthChanged?.Invoke();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
    }

    private void PlayerDied()
    {
        if (PlayerMovement.instance != null)
        {
            PlayerMovement.instance.gameObject.SetActive(false);
            deathScreen.SetActive(true);
        }

    }

    public bool IsAtFullHealth()
    {
        return currentHealth >= maxHealth;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    public void GiveInv()
    {
        invCount = damageInvLength;
    }
}

