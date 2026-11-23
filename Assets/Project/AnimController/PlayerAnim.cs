using UnityEngine;
namespace ClaseProgAvanzada
{
    
public class PlayerAnim : MonoBehaviour
{
    // Controlador de animacion
    [Header("Controlador de animacion")]
    [SerializeField] private Animator animController;
    [SerializeField] private PlayerMovement playerMovement;
    private bool activeJump = false;

    private void Awake()
    {
        if (animController == null) animController = GetComponent<Animator>();
        if (playerMovement == null) playerMovement = GetComponentInParent<PlayerMovement>();
        if (animController != null) animController.applyRootMotion = false;
    }

    public void UpdateAnim(bool onGround, Vector2 dirInput)
    {
         // Metodo que permite actulizar las animaciones
        if (animController == null) return;
        float currentSpeed = Mathf.Clamp01(dirInput.magnitude);
        animController.SetBool("OnGround", onGround);
        animController.SetFloat("Speed", currentSpeed);
        animController.SetFloat("Vertical", dirInput.y, 0.1f, Time.deltaTime);
        animController.SetFloat("Turning Direction", dirInput.x, 0.1f, Time.deltaTime);
    }


    public bool Land()
    {
        if (animController == null) return false;
        AnimatorStateInfo state = animController.GetCurrentAnimatorStateInfo(0);
        return state.IsName("JumpEnd_Bow") && state.normalizedTime < 0.9f;
    }
    public void Jump()
    {
        if (activeJump || animController == null) return;
        activeJump = true;
        animController.SetTrigger("Jump");


    }
    public void JumpEvent(){
        playerMovement?.JumpEvent();
    }

    public void JumpEndEvent(){
        activeJump = false;
    }

}

}
