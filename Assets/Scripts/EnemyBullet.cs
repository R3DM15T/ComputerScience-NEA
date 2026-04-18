using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float damage = 1f;
    private Vector3 direction;

    void Start()
    {
        direction = PlayerMovement.instance.transform.position - transform.position;
        direction.Normalize();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platforms")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            Destroy(gameObject);
            HealthController.instance.DamageTaken(damage);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
