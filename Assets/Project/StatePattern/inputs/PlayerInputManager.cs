using UnityEngine;

namespace ClaseProgAvanzada
{
    public class PlayerInputManager {

        //Patron Singleton
        private static PlayerMoveControl inputAction;


        public static PlayerMoveControl GetInputActions() {
            if (inputAction == null) {
                inputAction = new PlayerMoveControl();
                inputAction.Enable();
            }
            return inputAction;
            
            
        }
        public static void EnablePlayer() {
            GetInputActions().Character.Enable();
            
        }
        public static void DisablePlayer() {
            GetInputActions().Character.Disable();
            inputAction = null;
        }
        public static void clear()
        {
            inputAction = null;
        }
    }
    
  
}


