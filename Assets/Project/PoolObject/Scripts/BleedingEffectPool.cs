
using UnityEngine;
using UnityEngine.Pool;

namespace ClaseProgAvanzada
{
    public class BleedingEffectPool : MonoBehaviour
    {
        [SerializeField] private GameObject effectPrefab;
        [SerializeField] private int defaultPoolSize = 4;
        [SerializeField] private int maxPoolSize = 8;

        private ObjectPool<BleedingEffect> pool;

        public static BleedingEffectPool InstanceBleedingEffectPool
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (InstanceBleedingEffectPool == null)
            {
                InstanceBleedingEffectPool = this;
                pool = CreatePool();
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            BleedingEffect[] effects =
                new BleedingEffect[defaultPoolSize];

            for (int i = 0; i < defaultPoolSize; i++)
            {
                effects[i] = pool.Get();
            }

            foreach (BleedingEffect effect in effects)
            {
                pool.Release(effect);
            }
        }

        private ObjectPool<BleedingEffect> CreatePool()
        {
            return new ObjectPool<BleedingEffect>(
                createFunc: () =>
                {
                    GameObject go = Instantiate(effectPrefab, transform);

                    go.SetActive(false);

                    if (!go.TryGetComponent(out BleedingEffect effect))
                    {
                        effect = go.AddComponent<BleedingEffect>();
                    }

                    return effect;
                },

                actionOnGet: (effect) =>
                {
                    // Se activa despues de posicionarlo
                },

                actionOnRelease: (effect) =>
                {
                    effect.gameObject.SetActive(false);
                },

                actionOnDestroy: (effect) =>
                {
                    if (effect != null)
                        Destroy(effect.gameObject);
                },

                collectionCheck: true,
                defaultCapacity: defaultPoolSize,
                maxSize: maxPoolSize
            );
        }

        public BleedingEffect GetEffect(
            Vector3 position, Quaternion rotation)
        {
            BleedingEffect effect = pool.Get();

            effect.transform.SetPositionAndRotation(
                position, rotation);

            effect.gameObject.SetActive(true);

            return effect;
        }

        public void ReturnEffect(BleedingEffect effect)
        {
            if (effect != null && effect.gameObject.activeSelf)
            {
                pool.Release(effect);
            }
        }

        private void OnDestroy()
        {
            if (InstanceBleedingEffectPool == this)
                InstanceBleedingEffectPool = null;
        }
    }
}
