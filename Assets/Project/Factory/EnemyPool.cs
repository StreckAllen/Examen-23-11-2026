
using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [SerializeField] private EnemyFactory factory;

    private Dictionary<EnemyType, Queue<Enemy>> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            pools = new Dictionary<EnemyType, Queue<Enemy>>();

            pools.Add(EnemyType.Goblin, new Queue<Enemy>());
            pools.Add(EnemyType.Orc, new Queue<Enemy>());
            pools.Add(EnemyType.Skeleton, new Queue<Enemy>());
            pools.Add(EnemyType.Golem, new Queue<Enemy>());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Enemy GetEnemy(
        EnemyType type,
        Vector3 position,
        Quaternion rotation)
    {
        Enemy enemy;

        if (pools[type].Count > 0)
        {
            enemy = pools[type].Dequeue();
        }
        else
        {
            if (factory == null)
            {
                Debug.LogError("Falta asignar EnemyFactory");
                return null;
            }

            enemy = factory.CreateEnemy(type);

            if (enemy == null)
                return null;

            enemy.transform.SetParent(transform);
        }

        enemy.Initialize(type, position, rotation);

        return enemy;
    }

    public void ReturnEnemy(Enemy enemy)
    {
        if (enemy == null || !enemy.gameObject.activeSelf)
            return;

        enemy.gameObject.SetActive(false);
        pools[enemy.Type].Enqueue(enemy);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
