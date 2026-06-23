using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float attackInterval = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(
            nameof(Attack),
            0f,
            attackInterval
        );
    }
    void Attack()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
            return;

        GameObject nearestEnemy = enemies[0];

        float nearestDistance =
            Vector2.Distance(
                transform.position,
                nearestEnemy.transform.position
            );

        foreach (GameObject enemy in enemies)
        {
            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

        bullet.GetComponent<Bullet>()
              .SetTarget(nearestEnemy.transform);
    }
    
}
