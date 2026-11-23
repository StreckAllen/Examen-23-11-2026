
using UnityEngine;
using UnityEngine.Pool;

namespace ClaseProgAvanzada
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private GameObject projectilepref;
        [SerializeField] private int defaultPoolSize = 4;
        [SerializeField] private int maxPoolSize = 8;
        private ObjectPool<Projectile> pool;

        public static ProjectilePool InstanceProjectilePool
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (InstanceProjectilePool == null)
            {
                InstanceProjectilePool = this;
                pool = CreatePool();
                return;
            }

            Destroy(gameObject);
        }
        private ObjectPool<Projectile> CreatePool()
        {
            return new ObjectPool<Projectile>(
                createFunc: () =>
                {
                    GameObject go = Instantiate(
                        projectilepref, transform);

                    go.SetActive(false);

                    if (!go.TryGetComponent(out Projectile projectile))
                    {
                        projectile = go.AddComponent<Projectile>();
                    }

                    return projectile;
                },

                actionOnGet: (projectile) =>
                {
                    projectile.OnTakenFromPool();
                },

                actionOnRelease: (projectile) =>
                {
                    projectile.OnReturnedToPool();
                    projectile.gameObject.SetActive(false);
                },

                actionOnDestroy: (projectile) =>
                {
                    if (projectile != null)
                        Destroy(projectile.gameObject);
                },

                collectionCheck: true,
                defaultCapacity: defaultPoolSize,
                maxSize: maxPoolSize
            );
        }

        public Projectile GetProjectile(
            Vector3 position, Quaternion rotation)
        {
            Projectile projectile = pool.Get();

            projectile.transform.SetPositionAndRotation(
                position, rotation);

            projectile.gameObject.SetActive(true);

            return projectile;
        }

        public void ReturnProjectile(Projectile projectile)
        {
            if (projectile != null && projectile.gameObject.activeSelf)
            {
                pool.Release(projectile);
            }
        }

        private void OnDestroy()
        {
            if (InstanceProjectilePool == this)
                InstanceProjectilePool = null;
        }
    }
}
