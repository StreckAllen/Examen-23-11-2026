
using UnityEngine;
using System.Collections.Generic;

public class CollectiblePool : MonoBehaviour
{
    public static CollectiblePool Instance
    {
        get;
        private set;
    }

    [SerializeField]
    private CollectibleFactory factory;

    private Dictionary<TypeCoin, Queue<Coin>> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            pools = new Dictionary<TypeCoin, Queue<Coin>>();

            pools.Add(TypeCoin.Gold, new Queue<Coin>());
            pools.Add(TypeCoin.Silver, new Queue<Coin>());
            pools.Add(TypeCoin.Trophy, new Queue<Coin>());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Coin GetCoin(
        TypeCoin type,
        Vector3 position,
        Quaternion rotation)
    {
        Coin coin;

        if (pools[type].Count > 0)
        {
            coin = pools[type].Dequeue();
        }
        else
        {
            if (factory == null)
            {
                Debug.LogError(
                    "Falta asignar CollectibleFactory"
                );

                return null;
            }

            coin = factory.CreateCoin(type);

            if (coin == null)
                return null;

            coin.transform.SetParent(transform);
        }

        coin.Initialize(type, position, rotation);

        return coin;
    }

    public void ReturnCoin(Coin coin)
    {
        if (coin == null || !coin.gameObject.activeSelf)
            return;

        coin.gameObject.SetActive(false);

        pools[coin.Type].Enqueue(coin);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
