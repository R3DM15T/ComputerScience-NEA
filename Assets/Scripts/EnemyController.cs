using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] GameObject enemyHurtEffect;
    [SerializeField] GameObject coin;
 
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        Instantiate(enemyHurtEffect, transform.position, transform.rotation);
        if(health <= 0)
        {
            Instantiate(coin, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
