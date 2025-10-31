using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _accelerationTime = 0.3f;

    public float groundDrag;

    [Header("GroundChecker")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    
    public Transform orientation;
    
    private float _horizontalInput;
    private float _verticalInput;
    
    private Vector3 _moveDirection = Vector3.zero;
    
    private float _currentSpeedFactor;
    
    private Rigidbody _rigidbody;
    public bool isMoving;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
        
        InputUser();
        SpeedControl();
        UpdateAcceleration();
        
        _rigidbody.drag = _isGrounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InputUser()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        bool inputActive = _horizontalInput != 0 || _verticalInput != 0;
        
        if (!isMoving && inputActive)
        {
            isMoving = true;
        }
        else if (isMoving && !inputActive)
        {
            isMoving = false;
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
        _rigidbody.AddForce(_moveDirection.normalized * _movementSpeed, ForceMode.Force);
        
        float targetSpeed = _movementSpeed * _currentSpeedFactor;
        _rigidbody.AddForce(_moveDirection.normalized * targetSpeed, ForceMode.Force);
    }
    
    private void UpdateAcceleration()
    {
        _currentSpeedFactor = isMoving ? Mathf.MoveTowards(_currentSpeedFactor, 1f, Time.deltaTime / _accelerationTime) : 0f;
    }

    private void SpeedControl()
    {
        Vector3 finalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        if (finalVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitVelocity = finalVelocity.normalized * _movementSpeed;
            _rigidbody.velocity = new Vector3(limitVelocity.x, _rigidbody.velocity.y, limitVelocity.z);
        }

        if (transform.position.y < _playerHeight)
        {
            
        }
    }
}
