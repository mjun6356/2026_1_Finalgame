using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    Transform target;

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision
                .GetComponent<Enemy>()
                .TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
