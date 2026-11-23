using UnityEngine;

namespace ClaseProgAvanzada
{
    public class IddleState : IEnemyState
    {
        private readonly EnemyController controller;
        private const float DetectionInterval = 0.25f;
        private float detectionTimer;

        public IddleState(EnemyController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            detectionTimer = 0f;
            controller.StopMoving();
        }

        public void Update()
        {
            detectionTimer += Time.deltaTime;
            if (detectionTimer < DetectionInterval) return;
            detectionTimer = 0f;

            // También detiene el agente si el NavMesh estuvo listo después de Enter.
            controller.StopMoving();
            if (!controller.CanNavigate || !controller.TryFindPlayer()) return;

            Vector3 offset = controller.playerRef.position - controller.transform.position;
            float range = Mathf.Max(0f, controller.rangeDetection);
            if (offset.sqrMagnitude <= range * range)
                controller.ChangeState(controller.chaseState);
        }

        public void Exit() { }
    }
}
