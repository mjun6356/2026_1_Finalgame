using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnInterval = 2f;

    public float spawnRadius = 10f;

    private void Start()
    {
        InvokeRepeating(
            nameof(SpawnEnemy),
            1f,
            spawnInterval
        );
    }

    void SpawnEnemy()
    {
        Vector2 randomPos =
            (Vector2)GameObject
            .FindGameObjectWithTag("Player")
            .transform.position
            + Random.insideUnitCircle.normalized
            * spawnRadius;

        Instantiate(
            enemyPrefab,
            randomPos,
            Quaternion.identity
        );
    }
}
