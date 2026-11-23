namespace ClaseProgAvanzada
{
    // Metodos que deben tener todos los estados del cofre.
    public interface IChestState
    {
        void OnEnter();
        void Update();
        void OnExit();
        void Interact();
    }
}
