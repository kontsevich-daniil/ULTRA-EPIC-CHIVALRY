using System;
using Controllers;
using UnityEngine;
using Zenject;

public class Billboard : MonoBehaviour
{
    private PlayerController _playerController;
    private Transform playerCamera;
    public bool lockYRotation = true;
    
    [Inject]
    private void Initialized(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void Start()
    {
        playerCamera = _playerController.transform;
    }

    void Update()
    {
        /*if (playerCamera == null)
            return;

        Vector3 direction = playerCamera.position;

        transform.LookAt(direction);*/
        
        if (playerCamera == null)
            return;

        // Направление от спрайта к точке взгляда игрока
        Vector3 direction = playerCamera.position - transform.position;

        if (lockYRotation)
            direction.y = 0f; // фиксируем вертикальный поворот

        // Если направление не нулевое — поворачиваем
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}