
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnZone : MonoBehaviour
{
    [System.Serializable]
    public class SpawnGroup
    {
        public EnemyType spawnOnStart;
        public List<Transform> spawnPoints;
    }

    [SerializeField]
    private List<SpawnGroup> enemyGroups;

    [SerializeField]
    private CollectibleSpawnZone collectibleZone;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || activated)
            return;

        if (EnemyPool.Instance == null)
            return;

        activated = true;

        foreach (SpawnGroup group in enemyGroups)
        {
            foreach (Transform point in group.spawnPoints)
            {
                if (point == null)
                    continue;

                EnemyPool.Instance.GetEnemy(
                    group.spawnOnStart,
                    point.position,
                    point.rotation
                );
            }
        }

        if (collectibleZone != null)
        {
            collectibleZone.Spawn();
        }

        Destroy(gameObject);
    }
}
