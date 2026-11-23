using UnityEngine;
using UnityEngine.InputSystem;

namespace ClaseProgAvanzada
{
    public class Chest : MonoBehaviour
    {
        [Header("Partes del cofre")]
        [SerializeField] private Transform lid;
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject lockVisual;

        [Header("Interaccion")]
        [SerializeField] private Transform playerRef;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private bool startLocked;

        [Header("Movimiento de la tapa")]
        [SerializeField] private float openAngle = 100f;
        [SerializeField] private float lidSpeed = 180f;

        public ClosedState closedState { get; private set; }
        public OpenState openState { get; private set; }
        public LockedState lockedState { get; private set; }
        public bool isLocked { get; private set; }

        private IChestState currentState;
        private Quaternion closedRotation;
        private Quaternion targetRotation;

        private void Awake()
        {
            closedState = new ClosedState(this);
            openState = new OpenState(this);
            lockedState = new LockedState(this);

            if (lid != null)
                closedRotation = lid.localRotation;
        }

        private void Start()
        {
            if (playerRef == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerRef = player.transform;
            }

            if (startLocked)
                ChangeState(lockedState);
            else
                ChangeState(closedState);
        }

        private void Update()
        {
            currentState?.Update();

            if (SimpleStartMenu.IsOpen || Time.timeScale == 0f) return;
            if (playerRef == null || Keyboard.current == null) return;

            float distance = Vector3.Distance(transform.position, playerRef.position);
            if (distance > interactionDistance) return;

            if (Keyboard.current.eKey.wasPressedThisFrame) Interact();
            if (Keyboard.current.lKey.wasPressedThisFrame) Lock();
            if (Keyboard.current.uKey.wasPressedThisFrame) Unlock();
        }

        public void ChangeState(IChestState newState)
        {
            if (newState == null || newState == currentState) return;

            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }

        public void Interact()
        {
            currentState?.Interact();
        }

        public void Lock()
        {
            // Solo se puede bloquear cuando esta cerrado.
            if (currentState == closedState)
                ChangeState(lockedState);
            else if (currentState == openState)
                Debug.Log("Primero cierra el cofre con E.");
        }

        public void Unlock()
        {
            if (currentState == lockedState)
                ChangeState(closedState);
        }

        public void SetVisuals(bool opened, bool locked)
        {
            isLocked = locked;
            targetRotation = closedRotation;

            if (opened)
                targetRotation = closedRotation * Quaternion.Euler(openAngle, 0f, 0f);

            if (content != null) content.SetActive(opened);
            if (lockVisual != null) lockVisual.SetActive(locked);
        }

        public void MoveLid()
        {
            if (lid == null) return;

            lid.localRotation = Quaternion.RotateTowards(
                lid.localRotation, targetRotation, lidSpeed * Time.deltaTime);
        }
    }
}
