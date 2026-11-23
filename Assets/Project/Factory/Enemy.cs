
using UnityEngine;

public enum EnemyType
{
    Goblin,
    Orc,
    Skeleton,
    Golem
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType type;

    private bool returned = false;

    public EnemyType Type => type;

    public void Initialize(
        EnemyType newType,
        Vector3 position,
        Quaternion rotation)
    {
        type = newType;
        returned = false;

        transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ReturnToPool();
    }

    // Punto común para el contacto con el jugador y los impactos de flechas.
    public void ReturnToPool()
    {
        if (returned || !gameObject.activeSelf) return;
        returned = true;

        if (EnemyPool.Instance != null)
            EnemyPool.Instance.ReturnEnemy(this);
        else
            gameObject.SetActive(false);
    }
}
