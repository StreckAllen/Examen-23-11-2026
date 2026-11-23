using UnityEngine;
using UnityEngine.InputSystem;

namespace ClaseProgAvanzada
{
    [RequireComponent(typeof(Camera))]
    public class SimplePlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool firstPerson;
        [SerializeField] private Vector3 thirdPersonOffset = new Vector3(0, 1.5f, 0);
        [SerializeField, Min(0.3f)] private float distance = 4f;
        [SerializeField] private Vector3 eyeOffset = new Vector3(0, 1.65f, 0.1f);
        [SerializeField, Min(0f)] private float mouseSensitivity = 0.15f;
        [Tooltip("Capas que pueden bloquear la cámara. Los triggers se ignoran.")]
        [SerializeField] private LayerMask collisionLayers = ~(1 << 3);

        private float yaw;
        private float pitch = 18f;
        private bool previousMode;
        private Transform cachedTarget;
        private Renderer[] model;
        private bool[] originalVisibility;

        private void Awake()
        {
            GetComponent<Camera>().nearClipPlane = 0.05f;
            previousMode = firstPerson;
            pitch = firstPerson ? 0f : 18f;
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
                firstPerson = !firstPerson;

            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
                Vector2 look = Mouse.current.delta.ReadValue() * mouseSensitivity;
                yaw += look.x;
                pitch = Mathf.Clamp(pitch - look.y, -70f, 75f);
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                var player = FindAnyObjectByType<PlayerMovement>();
                if (player != null) target = player.transform;
            }
            if (target != cachedTarget)
            {
                RestoreModel();
                cachedTarget = target;
                model = target != null ? target.GetComponentsInChildren<Renderer>(true) : null;
                originalVisibility = model != null ? new bool[model.Length] : null;
                if (model != null)
                    for (int i = 0; i < model.Length; i++) originalVisibility[i] = model[i].forceRenderingOff;
            }
            if (target == null) return;
            if (previousMode != firstPerson)
            {
                yaw = 0f;
                pitch = firstPerson ? 0f : 18f;
                previousMode = firstPerson;
            }

            // Oculta el modelo solo en primera persona, sin desactivar sus scripts ni colliders.
            for (int i = 0; i < model.Length; i++)
                if (model[i] != null) model[i].forceRenderingOff = firstPerson || originalVisibility[i];

            Quaternion rotation = Quaternion.Euler(pitch, target.eulerAngles.y + yaw, 0f);
            Vector3 pivot = target.TransformPoint(firstPerson ? eyeOffset : thirdPersonOffset);
            float cameraDistance = firstPerson ? 0f : distance;
            if (!firstPerson)
            {
                // Acerca la cámara cuando una pared queda entre ella y el jugador.
                foreach (RaycastHit hit in Physics.SphereCastAll(pivot, 0.2f,
                    -(rotation * Vector3.forward), distance, collisionLayers, QueryTriggerInteraction.Ignore))
                {
                    if (!hit.transform.IsChildOf(target))
                        cameraDistance = Mathf.Min(cameraDistance, Mathf.Max(0f, hit.distance - 0.05f));
                }
            }
            transform.SetPositionAndRotation(pivot - rotation * Vector3.forward * cameraDistance, rotation);
        }

        private void OnDisable() => RestoreModel();

        private void RestoreModel()
        {
            if (model == null) return;
            for (int i = 0; i < model.Length; i++)
                if (model[i] != null) model[i].forceRenderingOff = originalVisibility[i];
        }
    }
}
