
using UnityEngine;
using System.Collections.Generic;

public class CollectibleFactory : MonoBehaviour
{
    [System.Serializable]
    public class CollectiblePrefab
    {
        public TypeCoin type;
        public GameObject prefab;
    }

    [SerializeField]
    private List<CollectiblePrefab> collectiblePrefabs;

    public Coin CreateCoin(TypeCoin type)
    {
        foreach (CollectiblePrefab entry in collectiblePrefabs)
        {
            if (entry.type == type && entry.prefab != null)
            {
                GameObject obj = Instantiate(
                    entry.prefab,
                    transform.position,
                    Quaternion.identity
                );

                obj.SetActive(false);

                Coin coin = obj.GetComponent<Coin>();

                if (coin == null)
                {
                    coin = obj.AddComponent<Coin>();
                }

                return coin;
            }
        }

        Debug.LogWarning(
            "No existe prefab para: " + type
        );

        return null;
    }
}
