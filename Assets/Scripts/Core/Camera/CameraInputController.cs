using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Camera
{
    public class CameraInputController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private InputActionReference panAction;
        [SerializeField] private InputActionReference panHoldAction;
        [SerializeField] private InputActionReference rotateAction;
        [SerializeField] private InputActionReference zoomAction;

        public Vector2 PanDelta { get; private set; }
        public bool IsPanning { get; private set; }
        public bool IsRotating { get; private set; }
        public float Zoom { get; private set; }

        private void OnEnable()
        {
            panAction?.action.Enable();
            panHoldAction?.action.Enable();
            rotateAction?.action.Enable();
            zoomAction?.action.Enable();
        }

        private void OnDisable()
        {
            panAction?.action.Disable();
            panHoldAction?.action.Disable();
            rotateAction?.action.Disable();
            zoomAction?.action.Disable();
        }

        private void Update()
        {
            PanDelta = panAction.action.ReadValue<Vector2>();
            IsPanning = panHoldAction.action.IsPressed();
            IsRotating = rotateAction.action.IsPressed();
            Zoom = zoomAction.action.ReadValue<float>();
        }
    }
}
