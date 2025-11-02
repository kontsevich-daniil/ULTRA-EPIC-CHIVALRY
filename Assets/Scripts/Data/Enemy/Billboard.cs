using System;
using Controllers;
using UnityEngine;
using Zenject;

public class Billboard : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Transform playerCamera;

    public bool lockYRotation = true;
    
    [Inject]
    private void Initialized(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    private void Start()
    {
        playerCamera = _playerMovement.transform;
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