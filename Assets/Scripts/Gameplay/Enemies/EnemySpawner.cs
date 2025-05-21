using System.Collections;
using UnityEngine;
using GameMechanic;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    public int TotalSpawnCount => IsInfinite ? 0 : spawnCount * waveCount;

    [Header("References")]
    public GameObject Character;
    public EnemyPool pool;
    public Transform spawnPoint;

    [Header("Wave Settings")]
    public float spawnInterval = 2f;
    public int spawnCount = 5;
    public int waveCount = 3;
    public float waveInterval = 5f;
    public bool IsInfinite = false;
    
    [Header("Enemy Settings")]
    [SerializeField] private int enemyTypeID;
  

    private void Awake()
    {
        pool.InitializeIfNeeded();
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        int currentWave = 0;

        while (IsInfinite || currentWave < waveCount)
        {
            yield return StartCoroutine(SpawnEnemies());
            currentWave++;

            if (!IsInfinite && currentWave >= waveCount)
                break;

            yield return new WaitForSeconds(waveInterval);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = pool.Get(enemyTypeID, spawnPoint);
            if (enemy == null) continue;

            enemy.Target = Character.transform;
            enemy.EnemyTypeID = enemyTypeID;
            enemy.Pool = pool;
            enemy.ResetState();


            yield return new WaitForSeconds(spawnInterval);
        }
    }
}