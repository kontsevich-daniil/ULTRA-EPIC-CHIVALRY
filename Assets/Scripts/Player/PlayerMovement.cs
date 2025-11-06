using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 8f;
    [SerializeField] private float _accelerationTime = 0.3f;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private float _maxSpeed = 10f;

    private bool _readyToJump = true;

    [Header("Keybinds")]
    private KeyCode _jumpKey = KeyCode.Space;

    [Header("GroundChecker")]
    [SerializeField] private Transform _playerIsGroundChecker;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection = Vector3.zero;

    private float _currentSpeedFactor;

    private Rigidbody _rigidbody;
    public bool isMoving;
    public bool isStopController = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        if (isStopController)
            return;

        _isGrounded = Physics.OverlapSphere(_playerIsGroundChecker.position, 0.2f, _groundLayer).Length > 0;

        InputUser();
        UpdateAcceleration();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void InputUser()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        bool inputActive = _horizontalInput != 0 || _verticalInput != 0;

        isMoving = inputActive;

        if (Input.GetKey(_jumpKey) && _readyToJump && _isGrounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), 0.5f);
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (isMoving)
        {
            float targetSpeed = _movementSpeed * _currentSpeedFactor;

            _rigidbody.AddForce(_moveDirection.normalized * targetSpeed, ForceMode.Acceleration);
        }
        else
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
    }

    private void UpdateAcceleration()
    {
        _currentSpeedFactor = isMoving ? Mathf.MoveTowards(_currentSpeedFactor, 1f, Time.deltaTime / _accelerationTime) : 0f;
    }

    private void SpeedControl()
    {
        Vector3 horizontalVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

        if (horizontalVelocity.magnitude > _maxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * _maxSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    public void StopController()
    {
        isStopController = true;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }

    public void StartController()
    {
        isStopController = false;
        _rigidbody.isKinematic = false;
    }
}
