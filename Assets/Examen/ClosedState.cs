using UnityEngine;

namespace ClaseProgAvanzada
{
    public class ClosedState : IChestState
    {
        private Chest chest;

        public ClosedState(Chest chest)
        {
            this.chest = chest;
        }

        public void OnEnter()
        {
            chest.SetVisuals(false, false);
            Debug.Log("Cofre cerrado.");
        }

        public void Update()
        {
            chest.MoveLid();
        }

        public void OnExit() { }

        public void Interact()
        {
            chest.ChangeState(chest.openState);
        }
    }
}
