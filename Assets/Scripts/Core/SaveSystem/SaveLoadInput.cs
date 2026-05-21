using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.SaveSystemm
{
    public class SaveLoadInput : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private InputActionReference saveAction;
        [SerializeField] private InputActionReference loadAction;

        public event SaveLoadDelegate OnSave;
        public event SaveLoadDelegate OnLoad;
        public delegate void SaveLoadDelegate();


        private void OnEnable()
        {
            saveAction?.action.Enable();
            loadAction?.action.Enable();

            saveAction.action.performed += SaveAction_OnPerformed;
            loadAction.action.performed += LoadAction_OnPerformed;
        }

        private void OnDisable()
        {
            saveAction?.action.Disable();
            loadAction?.action.Disable();

            saveAction.action.performed -= SaveAction_OnPerformed;
            loadAction.action.performed -= LoadAction_OnPerformed;
        }
     
        private void SaveAction_OnPerformed(InputAction.CallbackContext ctx)
        {
            OnSave?.Invoke();
        }

        private void LoadAction_OnPerformed(InputAction.CallbackContext ctx)
        {
            OnLoad?.Invoke();
        }
    }
}
