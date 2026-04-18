using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] GameObject enemyHurtEffect;
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
            Destroy(gameObject);
        }
    }
}
