using UnityEngine;

namespace ClaseProgAvanzada {

    // Componente requerido para el funcionamiento del script
    [RequireComponent(typeof(CharacterController))]

    public class PlayerMoveCharController : PlayerMovement {

        // Gravedad sobre conexion a tierra
        [SerializeField] private float minFallVelocity = -2.5f;

        // Referencia del componente character controller
        private CharacterController charController;

        // Gravedad general en el aire
        // private readonly float gravity = -9.81f;

        // Velocidad de caida
        private float fallVelocity;



        // Ciclo de vida de Unity, se emplea para gestionar y asignar componentes
        protected override void Awake() {
            base.Awake();
            charController = GetComponent<CharacterController>();
        }


        // Permite gestionar la gravedad del personaje
        protected override void HandleGravity() {
            float gravity = Physics.gravity.y;
            if (onGround && fallVelocity < 0f) {
                fallVelocity = minFallVelocity;
            }

            if (onGround && jump) {

                playerAnim.Jump();
            }

            // Aplicamos la misma aceleracion de Unity.
            fallVelocity += gravity * Time.deltaTime;

            // Movimiento horizontal.
            dirMove *= speedMove;

            // Movimiento vertical calculado.
            dirMove.y = fallVelocity;

            MovePlayer();
            ConsumeEvent();
        }
        public override void JumpEvent()
        {
            onGround = false;
            fallVelocity = jumpForce;
        }


        // Permite mover el controlador del personaje
        protected override void MovePlayer() {
            charController.Move(dirMove * Time.deltaTime);
        }


        // Permite obtener el movimiento del personaje
        protected override Vector3 GetMoveComponent => new (0f, fallVelocity, 0f);
    }
}