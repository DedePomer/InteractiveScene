using Core.Scene;
using Core.Ui;
using UnityEngine;

namespace Core.Camera
{
    [DisallowMultipleComponent, RequireComponent(typeof(CameraInputController))]
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform? target;

        [Header("Distance")]
        [SerializeField] private float orbitDistance = 3f;
        [SerializeField] private float zoomMinDistance = 1.5f;
        [SerializeField] private float zoomMaxDistance = 7f;

        [Header("Movment")]
        [SerializeField] private float cameraSpeed = 1.0f;
        [SerializeField] private float rotationSpeed = 3f;
        [SerializeField] private float zoomSpeed = 0.4f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;


        private CameraInputController _cameraInput;
        private float _yaw = 0;
        private float _pitch;

        private void Awake()
        {
            _cameraInput = GetComponent<CameraInputController>();
        }

        private void OnEnable()
        {
            SceneObjectListItemView.OnObjectSelectedEvent += ObjectChanged;
        }

        private void OnDisable()
        {
            SceneObjectListItemView.OnObjectSelectedEvent -= ObjectChanged;
        }

        private void Update()
        {
            Vector2 delta = _cameraInput.PanDelta;


            MoveCamera2D(delta);
            if (target != null)
            {
                RotateCamera(delta);
                HandleZoom();
            }
        }

        private Transform ObjectChanged(SceneObjectController obj)
        {
            target = obj.transform;
            Vector3 offset = new Vector3(0, 0, -orbitDistance);
            SetCameraPosition(offset);
            _yaw = 0;
            _pitch = 0;
            return target;
        }


        private void HandleZoom()
        {
            if (_cameraInput.Zoom == 0)
                return;

            orbitDistance -= _cameraInput.Zoom * zoomSpeed * Time.deltaTime;
            orbitDistance = Mathf.Clamp(orbitDistance, zoomMinDistance, zoomMaxDistance);

            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 offset = rotation * new Vector3(0, 0, -orbitDistance);
            SetCameraPosition(offset);
        }


        private void MoveCamera2D(Vector2 delta)
        {
            if (!_cameraInput.IsPanning)
                return;

            Vector3 move =
                (-transform.right * delta.x +
                 -transform.up * delta.y);

            Vector3 newPosition = transform.position + move * cameraSpeed * Time.deltaTime;

            transform.position = newPosition;
        }

        private void RotateCamera(Vector2 delta)
        {

            if (!_cameraInput.IsRotating)
                return;

            _yaw += delta.x * rotationSpeed;
            _pitch -= delta.y * rotationSpeed;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 offset = rotation * new Vector3(0, 0, -orbitDistance);


            SetCameraPosition(offset);
        }

        private void SetCameraPosition(Vector3 offset)
        {
            transform.position = target.position + offset;
            transform.LookAt(target.position);
        }
    }
}
