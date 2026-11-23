using UnityEngine;

namespace ClaseProgAvanzada
{
    public class OpenState : IChestState
    {
        private Chest chest;

        public OpenState(Chest chest)
        {
            this.chest = chest;
        }

        public void OnEnter()
        {
            chest.SetVisuals(true, false);
            Debug.Log("Cofre abierto.");
        }

        public void Update()
        {
            chest.MoveLid();
        }

        public void OnExit() { }

        public void Interact()
        {
            chest.ChangeState(chest.closedState);
        }
    }
}
