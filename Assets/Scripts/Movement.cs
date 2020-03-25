using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementForceFactor;
    public float rotationSpeedFactor;
    public float jumpSpeedFactor;
    
    private float _horizontalAxis;
    private float _verticalAxis;
    
    private bool _canJump;
    private bool _jumpLock;

    private Rigidbody _rigidbody;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _canJump = false;
        _jumpLock = false;
    }

    public void FixedUpdate()
    {
        Move();
        if (!_canJump) return;
        Jump();
        _canJump = false;
        _jumpLock = true;
    }

    public void Update()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump")) {
            _canJump = true;
        }
        Rotate();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Surface"))
        {
            _jumpLock = false;
        }
    }

    private void Rotate()
    {
        var rotationEulers = new Vector3(0, _horizontalAxis * Time.deltaTime * rotationSpeedFactor, 0);
        transform.Rotate(rotationEulers);
    }

    private void Move()
    {
        var movementVector = transform.forward.normalized * (_verticalAxis * movementForceFactor * Time.deltaTime);
        _rigidbody.AddForce(movementVector);
    }

    private void Jump()
    {
        if(_jumpLock) return;
        var jumpForce = Vector3.up * (Time.deltaTime * jumpSpeedFactor);
        _rigidbody.AddForce(jumpForce);
    }
}
