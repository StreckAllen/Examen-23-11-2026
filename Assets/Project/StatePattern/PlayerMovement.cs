using UnityEngine;

namespace ClaseProgAvanzada {

    // Script que permite controlar el movimiento del personaje
    public abstract class PlayerMovement : MonoBehaviour {

        [Header("Script animacion")]
        [SerializeField] protected PlayerAnim playerAnim;

        // Arma actual del personaje
        [SerializeField] private Weapon currentWeapon; 

        // Movimiento y Rotacion
        [Header("Ajuste de movimiento")]
        [SerializeField] protected float speedMove = 5f;
        [SerializeField] private float speedRotation = 120f;

        // Gravedad
        [Header("Ajuste de gravedad")]
        [SerializeField] protected float jumpForce = 5f;

        // Variables de reconocimiento de terreno "SphereCast"
        [Header("Ajuste de conexion a tierra")]
        [SerializeField] private float radiusDetectedGround = 0.2f;
        [SerializeField] private float groundCheckDistance = 0f;
        [SerializeField] private LayerMask ignoreLayer;


        // Si es verdadero se mantiene el boton de salto presionado, caso contrario no
        protected bool jump;

        // Si es verdadero esta conectado al suelo, caso contrario esta en el aire
        protected bool onGround = false;

        // Direcion de movimiento del personje en el entorno 3D
        protected Vector3 dirMove;

        // Almacena la direccion de entrada de los inputs
        private Vector2 dirInput = Vector2.zero;

        // Variables ocultas a nivel de inspector
        private Vector3 positionFoot;

        // Referencia del new input action
        private PlayerMoveControl inputActions;


        // Ciclo de vida de Unity, se emplea para gestionar y asignar componentes
        protected virtual void Awake() {
            
            if (playerAnim == null) playerAnim = GetComponentInChildren<PlayerAnim>(true);

        }


        // Metodo de llamada de Unity, se llama cuando el objeto se activa
        private void OnEnable() {
            inputActions = PlayerInputManager.GetInputActions();
            PlayerInputManager.EnablePlayer();
            SetupInputActionCallbacks();
        }


        // Metodo de llamada de Unity, se llama cuando el objeto se desactiva
        private void OnDisable() {
            DisableInputActionCallbacks();
            PlayerInputManager.DisablePlayer();
            PlayerInputManager.clear();
            
        }


        // Se llama en cada frame del dispositivo
        protected virtual void Update() {
            bool land = playerAnim != null && playerAnim.Land();
            if (land) {
                dirInput = Vector2.zero;
            }
            // Inputs
            if (!land) dirInput = inputActions.Character.Movement.ReadValue<Vector2>();
            // Rotacion
            dirMove = transform.forward * dirInput.y;
            float rotation = speedRotation * dirInput.x * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
            HandleGravity();
            // MovePlayer();
            playerAnim?.UpdateAnim(onGround, dirInput);
            // ConsumeEvent();
        }


        // Se llama 50 veces x seg.
        protected virtual void FixedUpdate() {
            positionFoot = transform.position + Vector3.down * groundCheckDistance;
            onGround = Physics.CheckSphere(
                positionFoot,
                radiusDetectedGround,
                ~ignoreLayer,
                QueryTriggerInteraction.Ignore);
        }


       
        
        


        // Configura los callbacks para las acciones de entrada
        private void SetupInputActionCallbacks() {
            //Acciones de salto
            inputActions.Character.Jump.performed += ctx => { jump = true; };
            inputActions.Character.Fire.performed += OnFire;

        }


        private void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!SimpleStartMenu.IsOpen) currentWeapon?.Use();
        }

        // Deshabilita los callbacks para las acciones de entrada
        private void DisableInputActionCallbacks() {
            // Acciones de salto
            inputActions.Character.Jump.performed  -= ctx => { jump = false; };
            inputActions.Character.Fire.performed -= OnFire;
        }

        
        // Metodo que permite consumir los eventos de entrada
        protected void ConsumeEvent() {
            jump = false;
        }


        public void Jump() {
            jump = true;
        }


        // Permite gestionar la gravedad del personaje
        protected abstract void HandleGravity();

        // Permite mover el controlador del personaje
        protected abstract void MovePlayer();

        //Controlador de animacion mediante event system
        public abstract void JumpEvent();

        // Permite obtener el movimiento del personaje
        protected abstract Vector3 GetMoveComponent { get; }


        // Permite dibujar el la escena el efecto visual del
        // detector de reconocimiento de terreno "CheckSphere"
    # if UNITY_EDITOR
        [SerializeField] private Color groundColor = Color.green;
        [SerializeField] private Color noGroundColor = Color.red;

        private void OnDrawGizmos() {
            // Operador ternario
            Gizmos.color = onGround ? groundColor: noGroundColor;
            positionFoot = transform.position + Vector3.down * groundCheckDistance;
            Gizmos.DrawSphere(positionFoot, radiusDetectedGround);
        }
    #endif
    }
}