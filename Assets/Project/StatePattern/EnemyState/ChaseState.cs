using UnityEngine;

namespace ClaseProgAvanzada
{
    public class ChaseState : IEnemyState
    {
        private readonly EnemyController controller;

        public ChaseState(EnemyController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            if (!controller.CanNavigate) return;
            ConfigureAgent();
        }

        public void Update()
        {
            if (controller.playerRef == null ||
                !controller.playerRef.gameObject.activeInHierarchy)
            {
                controller.ChangeState(controller.iddleState);
                return;
            }
            if (!controller.CanNavigate) return;

            Vector3 targetPosition = controller.playerRef.position;
            Vector3 offset = targetPosition - controller.transform.position;
            float distanceSqr = offset.sqrMagnitude;

            // Un rango de salida mayor evita alternar estados en el borde de detección.
            float maxRange = Mathf.Max(controller.rangeDetection, controller.maxDistanceTarget);
            if (controller.maxDistanceTarget > 0f && distanceSqr > maxRange * maxRange)
            {
                controller.ChangeState(controller.iddleState);
                return;
            }

            ConfigureAgent();
            float stopDistance = Mathf.Max(0f, controller.minDistanceTarget);
            if (distanceSqr <= stopDistance * stopDistance)
            {
                controller.StopMoving();
                return;
            }

            // Sigue al jugador y vuelve a moverse si este se aleja. No hay ataque.
            controller.agent.SetDestination(targetPosition);
        }

        private void ConfigureAgent()
        {
            controller.agent.speed = Mathf.Max(0f, controller.speedChase);
            controller.agent.stoppingDistance = Mathf.Max(0f, controller.minDistanceTarget);
            controller.agent.isStopped = false;
        }

        public void Exit()
        {
            controller.StopMoving();
        }
    }
}
