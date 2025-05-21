using System.Collections.Generic;
using UnityEngine;
using GameMechanic;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public int id;
        public EnemyController prefab;
        public int initialCount = 10;
    }

    [SerializeField] private List<EnemyEntry> enemyTypes = new List<EnemyEntry>();

    private Dictionary<int, Queue<EnemyController>> _pools = new();
    private Dictionary<int, EnemyController> _prefabMap = new();
    private bool _initialized = false;
    private void Awake()
    {

        foreach (var entry in enemyTypes)
        {
            _prefabMap[entry.id] = entry.prefab;
            var queue = new Queue<EnemyController>();
            for (int i = 0; i < entry.initialCount; i++)
            {
                var enemy = Instantiate(entry.prefab, transform);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            _pools[entry.id] = queue;
        }
    }

    public void InitializeIfNeeded()
    {
        if (_initialized) return;
        _initialized = true;

        foreach (var entry in enemyTypes)
        {
            _prefabMap[entry.id] = entry.prefab;
            var queue = new Queue<EnemyController>();
            for (int i = 0; i < entry.initialCount; i++)
            {
                var enemy = Instantiate(entry.prefab, transform);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            _pools[entry.id] = queue;
        }
    }

    public EnemyController Get(int id, Transform parent)
    {
        if (!_pools.ContainsKey(id))
        {
            Debug.LogError($"No pool for enemy type: {id}");
            return null;
        }

        var pool = _pools[id];

        if (pool.Count == 0)
        {
            var newEnemy = Instantiate(_prefabMap[id]);
            newEnemy.gameObject.SetActive(false);
            pool.Enqueue(newEnemy);
        }

        var enemyObj = pool.Dequeue();
    
        enemyObj.transform.SetParent(parent, false);
        enemyObj.transform.localPosition = Vector3.zero;
        enemyObj.transform.localRotation = Quaternion.identity;

        enemyObj.gameObject.SetActive(true);
        return enemyObj;
    }


    public void Return(int id, EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
        if (_pools.ContainsKey(id))
        {
            _pools[id].Enqueue(enemy);
        }
        else
        {
            Destroy(enemy.gameObject); 
        }
    }
}