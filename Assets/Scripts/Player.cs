using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] private float _playerHeight;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _stopSpeed;
	[SerializeField] private float _gravityForce;
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _minSlideAngle;
	[SerializeField] private float _slideForce;
	[SerializeField] private Transform _head;

	private Rigidbody _rigidbody;
	private PlayerInputActions _playerInputActions;
	
	private float _distanceToGround;

	private bool IsTouchingGround => _distanceToGround <= _playerHeight;
	private bool _isSliding;

	private void Awake()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		_rigidbody = GetComponent<Rigidbody>();

		_playerInputActions = new PlayerInputActions();
		_playerInputActions.Enable();

		_playerInputActions.Player.Jump.performed += Jump;
	}

	private void FixedUpdate()
	{
		UpdateDistanceToGround();

		Move(Time.fixedDeltaTime);
		ApplyGravity(Time.fixedDeltaTime);
		Slide(Time.fixedDeltaTime);
	}

	private void ApplyGravity(float deltaTime)
	{
		if (IsTouchingGround && !_isSliding && _rigidbody.velocity.y <= 0)
		{
			var velocity = _rigidbody.velocity;
			velocity.y = 0;
			_rigidbody.velocity = velocity;

			var currentPosition = _rigidbody.position;
			var targetPosition = currentPosition;
			targetPosition.y += _playerHeight - _distanceToGround;
			_rigidbody.position = Vector3.Lerp(currentPosition, targetPosition, 0.3f);
		}
		else
		{
			_rigidbody.velocity += Vector3.down * _gravityForce * deltaTime;
		}
	}

	private void Slide(float deltaTime)
	{
		_isSliding = false;
		if (Physics.Raycast(transform.position, Vector3.down, out var hit, _playerHeight))
		{
			var angle = Vector3.Angle(hit.normal, Vector3.up);
			if (angle > _minSlideAngle)
			{
				var directionParallelToGround = Vector3.Cross(hit.normal, Vector3.up);
				var directionAlongTheSlide = Vector3.Cross(hit.normal, directionParallelToGround);

				_rigidbody.velocity += directionAlongTheSlide * _slideForce * angle / 90f * deltaTime;

				_isSliding = true;
			}
		}
	}

	private void UpdateDistanceToGround()
	{
		if (Physics.Raycast(transform.position, Vector3.down, out var hit, _playerHeight))
		{
			_distanceToGround = hit.distance;
		}
		else
		{
			_distanceToGround = float.MaxValue;
		}
	}

	private void Move(float deltaTime)
	{
		var characterForwardDir = _head.forward;
		characterForwardDir.y = 0;
		characterForwardDir.Normalize();

		var moveInput = _playerInputActions.Player.Move.ReadValue<Vector2>();
		var currentVelocity = _rigidbody.velocity;
		var horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

		if (moveInput.magnitude == 0)
		{
			horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, _stopSpeed);
		}
		else
		{
			var newVelocity = (characterForwardDir * moveInput.y + _head.right * moveInput.x).normalized * _moveSpeed;

			horizontalVelocity = Vector3.Lerp(horizontalVelocity, newVelocity, _stopSpeed);
		}

		horizontalVelocity.y = currentVelocity.y;
		currentVelocity = horizontalVelocity;

		_rigidbody.velocity = currentVelocity;
	}

	private void Jump(InputAction.CallbackContext obj)
	{
		if (_distanceToGround <= _playerHeight + 0.1f)
		{
			_rigidbody.velocity += (Vector3.up + _rigidbody.velocity.normalized).normalized * _jumpForce;
		}
	}
}