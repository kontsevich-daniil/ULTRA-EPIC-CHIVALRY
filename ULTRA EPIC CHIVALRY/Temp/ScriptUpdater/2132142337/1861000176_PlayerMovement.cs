using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 5f;

    public float groundDrag;

    [Header("GroundChecker")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    
    public Transform orientation;
    
    private float _horizontalInput;
    private float _verticalInput;
    
    private Vector3 _moveDirection = Vector3.zero;
    
    private Rigidbody _rigidbody;

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
        
        _rigidbody.drag = _isGrounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InputUser()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Debug.unityLogger.Log(string.Format("Horizontal: {0}, Vertical: {1}", _horizontalInput, _verticalInput));
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
        _rigidbody.AddForce(_moveDirection.normalized * _movementSpeed, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 finalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        if (finalVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitVelocity = finalVelocity.normalized * _movementSpeed;
            _rigidbody.velocity = new Vector3(limitVelocity.x, _rigidbody.velocity.y, limitVelocity.z);
        }

        if (expr)
        {
            
        }
    }
}
