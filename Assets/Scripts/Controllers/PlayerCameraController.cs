using UnityEngine;

namespace Controllers
{
    public class PlayerCameraController : MonoBehaviour
    {
        private float _xRotation;
        private float _yRotation;

        [Header("Settings")] public float bobFrequency = 10f;
        public float bobAmplitude = 40f;
        public float smooth = 10f;

        [Header("References")] public PlayerMovement playerController;

        private float _timer;
    
        public float sensitivityX;
        public float sensitivityY;

        public Transform cameraTransform;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleHeadBob();
        }
    
        void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            // Применяем базовое вращение мышью
            transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, transform.localRotation.z);
            cameraTransform.localRotation = Quaternion.Euler(0, _yRotation, 0);
        }

        void HandleHeadBob()
        {
            float targetRoll = 0f;
            float targetPitch = 0f;

            if (playerController.isMoving)
            {
                _timer += Time.deltaTime * bobFrequency;
                float roll = Mathf.Sin(_timer) * bobAmplitude;
                float pitchOffset = Mathf.Cos(_timer * 2f) * bobAmplitude * 0.2f;

                Quaternion mouseRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
                Quaternion bobRotation = Quaternion.Euler(pitchOffset, 0f, roll);

                transform.localRotation = mouseRotation * bobRotation;
            }
            else
            {
                _timer = 0f;
            }
        }

        public void StopController()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            enabled = false;
        }
        
        public void StartController()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            enabled = true;
        }
    }
}