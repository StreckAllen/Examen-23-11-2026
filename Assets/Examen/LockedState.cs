using UnityEngine;

namespace ClaseProgAvanzada
{
    public class LockedState : IChestState
    {
        private Chest chest;

        public LockedState(Chest chest)
        {
            this.chest = chest;
        }

        public void OnEnter()
        {
            chest.SetVisuals(false, true);
            Debug.Log("Cofre bloqueado.");
        }

        public void Update()
        {
            chest.MoveLid();
        }

        public void OnExit() { }

        public void Interact()
        {
            Debug.Log("El cofre esta bloqueado. Presiona U para desbloquear.");
        }
    }
}
