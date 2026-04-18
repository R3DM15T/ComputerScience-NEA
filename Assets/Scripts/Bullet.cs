using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] int damage = 1;
    [SerializeField] GameObject impactEffect;
    public Rigidbody2D rigidBody;
    PlayerMovement player;
    float speed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        player = FindObjectOfType<PlayerMovement>();
        speed = bulletSpeed;
        
    }


    void Update()
    {
        rigidBody.linearVelocity = transform.right * speed; //give bullet a continuous force with direction
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Platforms")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(1);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
