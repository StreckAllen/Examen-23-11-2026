using UnityEngine;
using UnityEngine.EventSystems;

namespace ClaseProgAvanzada
{
    public class SimpleStartMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuRoot;
        public static bool IsOpen { get; private set; }

        private void OnEnable()
        {
            IsOpen = true;
            if (menuRoot != null) menuRoot.SetActive(true);
        }

        public void Play()
        {
            if (menuRoot != null) menuRoot.SetActive(false);
            IsOpen = false;
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        }

        private void OnDisable() => IsOpen = false;
    }
}
