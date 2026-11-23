
using UnityEngine;
using System.Collections;

namespace ClaseProgAvanzada
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float waitTime = 3f;
        [SerializeField] private float speed = 10f;
        [SerializeField] private Transform projectileTip;


        //solventa bug de impacto

        private bool returned;
        private bool impact;

        private void OnEnable()
        {
            impact= false;
            returned = false;
            StartCoroutine(WaitTime());
        }

        private void Update()
        {
            if (!returned)
            {
                transform.position +=
                    transform.forward * speed * Time.deltaTime;
            }
        }

        private IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(waitTime);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<Projectile>() != null) return; // Ignorar otras flechas.
            if (returned || impact) return;
            if (other.CompareTag("Player") || other.GetComponentInParent<PlayerMovement>() != null) return;

            // Reconoce al enemigo aunque el collider esté en un objeto hijo.
            Enemy enemy = other.GetComponentInParent<Enemy>();
            bool hitEnemy = enemy != null || other.CompareTag("Enemy");

            // Solo registra impactos aceptados: tocar al jugador no bloquea la flecha.
            impact = true;

            // Colocar el efecto sobre la superficie, no en la punta dentro del muro.
            Vector3 position = other.ClosestPoint(transform.position);
            Vector3 normal = (transform.position - position).normalized;
            if (other.Raycast(new Ray(transform.position - transform.forward * 2f,
                transform.forward), out RaycastHit hit, 4f))
            {
                position = hit.point;
                normal = hit.normal;
            }
            if (normal == Vector3.zero) normal = -transform.forward;
            position += normal * 0.03f;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

            if (hitEnemy)
            {
                if (BleedingEffectPool.InstanceBleedingEffectPool != null)
                {
                    BleedingEffectPool.InstanceBleedingEffectPool.GetEffect(
                        position, rotation);
                }

                if (enemy != null)
                    enemy.ReturnToPool();
            }
            else
            {
                if (ImpactEffectPool.InstanceImpactEffectPool != null)
                {
                    // Este prefab emite por su eje Z, perpendicular a la superficie.
                    ImpactEffectPool.InstanceImpactEffectPool.GetEffect(
                        position, Quaternion.FromToRotation(Vector3.forward, normal));
                }
            }

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (returned) return;

            returned = true;

            if (ProjectilePool.InstanceProjectilePool != null)
            {
                ProjectilePool.InstanceProjectilePool.ReturnProjectile(this);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public virtual void OnTakenFromPool()
        {
            returned = false;
        }

        public virtual void Initialize()
        {
        }

        public virtual void OnReturnedToPool()
        {
        }
    }
}
