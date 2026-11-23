
using UnityEngine;

public enum TypeCoin
{
    Gold,
    Silver,
    Trophy
}

public class Coin : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private TypeCoin type;

    private bool collected = false;

    public TypeCoin Type => type;

    public void Initialize(
        TypeCoin newType,
        Vector3 position,
        Quaternion rotation)
    {
        type = newType;
        collected = false;

        transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || collected)
            return;

        collected = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectCoin(type, amount);
        }

        if (CollectiblePool.Instance != null)
        {
            CollectiblePool.Instance.ReturnCoin(this);
        }
    }
}
