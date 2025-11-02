using Controllers;
using UnityEngine;
using Zenject;

namespace Data.Enemy
{
    public class BillboardProjectile: MonoBehaviour
    {
        private Transform playerCamera;

        public bool lockYRotation = true;

        private void Start()
        {
            playerCamera = FindObjectOfType<PlayerMovement>().orientation.transform;
        }

        void Update()
        {
            if (playerCamera == null)
                return;

            Vector3 direction = playerCamera.position - transform.position;

            if (lockYRotation)
                direction.y = 0f;

            if (direction == Vector3.zero) 
                return;
        
            transform.LookAt(direction);
        }
    }
}