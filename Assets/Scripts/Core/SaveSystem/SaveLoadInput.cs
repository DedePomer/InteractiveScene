using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.SaveSystemm
{
    public class SaveLoadInput : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private InputActionReference saveAction;
        [SerializeField] private InputActionReference loadAction;

        public static event SaveLoadDelegate OnSave;
        public static event SaveLoadDelegate OnLoad;
        public delegate void SaveLoadDelegate();


        private void OnEnable()
        {
            saveAction?.action.Enable();
            loadAction?.action.Enable();
        }

        private void OnDisable()
        {
            saveAction?.action.Disable();
            loadAction?.action.Disable();
        }

        private void Update()
        {
            Save();
            Load();
        }


        public void Save()
        {
            if (!saveAction.action.IsPressed())
                return;
            OnSave?.Invoke();
        }

        public void Load()
        {
            if (!loadAction.action.IsPressed())
                return;
            OnLoad?.Invoke();
        }
    }
}
