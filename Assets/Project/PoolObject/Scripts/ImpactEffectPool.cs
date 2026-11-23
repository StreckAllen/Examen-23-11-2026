using UnityEngine;
using System.Collections.Generic;

namespace ClaseProgAvanzada
{
    public class ImpactEffectPool : MonoBehaviour
    {
        [SerializeField] private GameObject effectPrefab;
        [Tooltip("Cantidad preparada al iniciar o cambiar los tamaños. No supera Max Pool Size.")]
        [SerializeField, Min(0)] private int defaultPoolSize = 8;
        [Tooltip("Máximo de efectos inactivos que se guardan. Se puede cambiar durante Play.")]
        [SerializeField, Min(1)] private int maxPoolSize = 8;

        private readonly Queue<ImpactEffect> pool = new Queue<ImpactEffect>();
        private int activeCount;
        private int appliedDefault = -1;
        private int appliedMax = -1;

        public int InactiveCount => pool.Count;
        public int ActiveCount => activeCount;
        public static ImpactEffectPool InstanceImpactEffectPool { get; private set; }

        private void Awake()
        {
            if (InstanceImpactEffectPool != null && InstanceImpactEffectPool != this)
            {
                Destroy(gameObject);
                return;
            }
            InstanceImpactEffectPool = this;
            pool.Clear();
            activeCount = 0;
            appliedDefault = appliedMax = -1;
        }

        private void Start() => ApplyPoolSizes();
        private void Update() => ApplyPoolSizes();

        private void ApplyPoolSizes()
        {
            if (InstanceImpactEffectPool != this) return;
            defaultPoolSize = Mathf.Max(0, defaultPoolSize);
            maxPoolSize = Mathf.Max(1, maxPoolSize);
            if (appliedDefault == defaultPoolSize && appliedMax == maxPoolSize) return;
            appliedDefault = defaultPoolSize;
            appliedMax = maxPoolSize;

            // Al reducir el límite, solo se eliminan efectos que ya están inactivos.
            while (pool.Count > maxPoolSize)
                Destroy(pool.Dequeue().gameObject);

            int target = Mathf.Min(defaultPoolSize, maxPoolSize);
            while (pool.Count + activeCount < target && effectPrefab != null)
                pool.Enqueue(CreateEffect());
        }

        private ImpactEffect CreateEffect()
        {
            GameObject go = Instantiate(effectPrefab, transform);
            go.SetActive(false);
            if (!go.TryGetComponent(out ImpactEffect effect))
                effect = go.AddComponent<ImpactEffect>();
            return effect;
        }

        public ImpactEffect GetEffect(Vector3 position, Quaternion rotation)
        {
            ApplyPoolSizes();
            if (pool.Count == 0 && effectPrefab == null)
            {
                Debug.LogError("Falta asignar el prefab de impacto.", this);
                return null;
            }
            ImpactEffect effect = pool.Count > 0 ? pool.Dequeue() : CreateEffect();
            activeCount++;
            effect.transform.SetPositionAndRotation(position, rotation);
            effect.gameObject.SetActive(true);
            return effect;
        }

        public void ReturnEffect(ImpactEffect effect)
        {
            if (effect == null || !effect.gameObject.activeSelf) return;
            effect.gameObject.SetActive(false);
            activeCount--;
            if (pool.Count < Mathf.Max(1, maxPoolSize))
                pool.Enqueue(effect);
            else
                Destroy(effect.gameObject);
        }

        private void OnDestroy()
        {
            if (InstanceImpactEffectPool == this)
                InstanceImpactEffectPool = null;
        }
    }
}
