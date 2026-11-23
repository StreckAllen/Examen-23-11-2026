
using UnityEngine;
using System.Collections.Generic;

public class CollectibleSpawnZone : MonoBehaviour
{
    [System.Serializable]
    public class SpawnGroup
    {
        public TypeCoin spawnOnStart;
        public List<Transform> spawnPoints;
    }

    [SerializeField]
    private List<SpawnGroup> collectibleGroups;

    [SerializeField]
    private bool spawnOnTrigger = true;

    private bool activated = false;

    public void Spawn()
    {
        if (activated || CollectiblePool.Instance == null)
            return;

        activated = true;

        foreach (SpawnGroup group in collectibleGroups)
        {
            foreach (Transform point in group.spawnPoints)
            {
                if (point == null)
                    continue;

                CollectiblePool.Instance.GetCoin(
                    group.spawnOnStart,
                    point.position,
                    point.rotation
                );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!spawnOnTrigger)
            return;

        if (!other.CompareTag("Player"))
            return;

        Spawn();
    }
}
