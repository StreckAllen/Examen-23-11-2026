
using UnityEngine;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPrefab
    {
        public EnemyType type;
        public GameObject prefab;
    }

    [SerializeField] private List<EnemyPrefab> enemyPrefabs;

    public Enemy CreateEnemy(EnemyType type)
    {
        foreach (EnemyPrefab entry in enemyPrefabs)
        {
            if (entry.type == type && entry.prefab != null)
            {
                GameObject obj = Instantiate(
                    entry.prefab,
                    transform.position,
                    Quaternion.identity
                );

                obj.SetActive(false);

                Enemy enemy = obj.GetComponent<Enemy>();

                if (enemy == null)
                    enemy = obj.AddComponent<Enemy>();

                return enemy;
            }
        }

        Debug.LogWarning("No existe prefab para " + type);
        return null;
    }
}
