
using UnityEngine;
using CartoonFX;
using System.Collections;

namespace ClaseProgAvanzada
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float waitTime = 2f;

        private ParticleSystem[] particles;

        private void Awake()
        {
            foreach (CFXR_Effect effect in GetComponentsInChildren<CFXR_Effect>(true))
                effect.clearBehavior = CFXR_Effect.ClearBehavior.None;

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

            if (ImpactEffectPool.InstanceImpactEffectPool != null)
            {
                ImpactEffectPool.InstanceImpactEffectPool.ReturnEffect(this);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
