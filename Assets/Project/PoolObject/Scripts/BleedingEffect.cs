
using CartoonFX;
using UnityEngine;
using System.Collections;

namespace ClaseProgAvanzada
{
    public class BleedingEffect : MonoBehaviour
    {
        [SerializeField] private float waitTime = 2f;

        private ParticleSystem[] particles;

        private void Awake()
        {
            // El pool controla la devolucion; Cartoon FX no debe destruir ni
            // desactivar el objeto antes de que termine WaitTime().
            foreach (CFXR_Effect effect in GetComponentsInChildren<CFXR_Effect>(true))
            {
                effect.clearBehavior = CFXR_Effect.ClearBehavior.None;
            }

            particles = GetComponentsInChildren<ParticleSystem>(true);

            foreach (ParticleSystem particle in particles)
            {
                var main = particle.main;
                main.stopAction = ParticleSystemStopAction.None;
                main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            }
        }

        private void OnEnable()
        {
            if (particles == null)
                particles = GetComponentsInChildren<ParticleSystem>(true);

            foreach (ParticleSystem particle in particles)
            {
                particle.Stop(false,
                    ParticleSystemStopBehavior.StopEmittingAndClear);

                particle.Play(false);
            }

            StartCoroutine(WaitTime());
        }

        private IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(waitTime);
            // Devolver solo cuando también hayan terminado las partículas hijas.
            yield return new WaitUntil(() => !System.Array.Exists(particles,
                particle => particle != null && particle.IsAlive(false)));

            if (BleedingEffectPool.InstanceBleedingEffectPool != null)
            {
                BleedingEffectPool.InstanceBleedingEffectPool.ReturnEffect(this);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
