using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.SaveSystemm
{
    public class SaveManagerInput : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private InputActionReference saveAction;
        [SerializeField] private InputActionReference loadAction;


        public bool IsLoad { get; private set; }
        public bool IsSave { get; private set; }


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
            IsLoad = loadAction.action.IsPressed();
            IsSave = saveAction.action.IsPressed();
        }
    }
}
