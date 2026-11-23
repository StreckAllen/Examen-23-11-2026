using UnityEngine;
using UnityEngine.AI;

namespace ClaseProgAvanzada
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Tooltip("Si está vacío, busca un objeto activo con el tag Player.")]
        public Transform playerRef;
        [Min(0f)] public float rangeDetection = 10f;
        [Min(0f)] public float speedChase = 4f;
        [Tooltip("Distancia al jugador a la que deja de avanzar. No ataca.")]
        [Min(0f)] public float minDistanceTarget = 2f;
        [Tooltip("Distancia para abandonar la persecución. 0 = seguir sin límite. Usa un valor mayor que Range Detection.")]
        [Min(0f)] public float maxDistanceTarget = 15f;

        public NavMeshAgent agent;
        public IddleState iddleState { get; private set; }
        public ChaseState chaseState { get; private set; }
        private IEnemyState currentState;
        private Animator animator;

        public bool CanNavigate => agent != null &&
            agent.isActiveAndEnabled && agent.isOnNavMesh;

        private void Awake()
        {
            // El agente debe controlar el mismo objeto que el controlador.
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            iddleState = new IddleState(this);
            chaseState = new ChaseState(this);
        }

        private void OnEnable()
        {
            // OnEnable también se ejecuta al reutilizar un enemigo del pool.
            TryFindPlayer();
            ChangeState(iddleState);
        }

        private void OnDisable()
        {
            StopMoving();
            currentState?.Exit();
            currentState = null;
        }

        private void Update()
        {
            if (!CanNavigate) return;
            currentState?.Update();
        }

        private void LateUpdate()
        {
            if (animator != null && animator.runtimeAnimatorController != null)
                animator.SetFloat("Speed", CanNavigate ? agent.velocity.magnitude : 0f);
        }

        public bool TryFindPlayer()
        {
            if (playerRef != null && playerRef.gameObject.activeInHierarchy)
                return true;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerRef = player != null ? player.transform : null;
            return playerRef != null;
        }

        public void StopMoving()
        {
            if (!CanNavigate) return;
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }

        public void ChangeState(IEnemyState newState)
        {
            if (newState == null || newState == currentState) return;
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
