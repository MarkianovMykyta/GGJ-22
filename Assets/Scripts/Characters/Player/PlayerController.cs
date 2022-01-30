using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float _playerHeight;
		[SerializeField] private float _crouchingHeight;
		[SerializeField] private float _moveSpeed;
		[SerializeField] private float _stopSpeed;
		[SerializeField] private float _gravityForce;
		[SerializeField] private float _jumpForce;
		[SerializeField] private float _minSlideAngle;
		[SerializeField] private float _slideForce;
		[SerializeField] private float _standForce;
		[SerializeField] private float _gravityCompensation;
		[SerializeField] private float _interactDistance;
		[SerializeField] private Transform _head;
		[SerializeField] private float _period;
		[SerializeField] private AnimationCurve _curve;

		private Rigidbody _rigidbody;
		private PlayerInputActions _playerInputActions;

		private float _currentHeight;
		private float _currentSpeed;
		private float _distanceToGround;

		private bool IsTouchingGround => _distanceToGround <= _currentHeight;
		private bool _isSliding;

		private int _footIndex;
		private int _jumpIndex;

		[SerializeField] private SoundManager _soundManager; //temporary

		public void Initialize(SoundManager soundManager)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			_rigidbody = GetComponent<Rigidbody>();

			_playerInputActions = new PlayerInputActions();
			_playerInputActions.Enable();

			_playerInputActions.Player.Jump.performed += Jump;
			_playerInputActions.Player.Interact.performed += Interact;

			_soundManager = soundManager;
			_footIndex = _soundManager.GetAudioClipIndex("step");
			_jumpIndex = _soundManager.GetAudioClipIndex("jump");
		}

		public void Freeze()
		{
			_playerInputActions.Disable();
		}

		public void UnFreeze()
		{
			_playerInputActions.Enable();
		}

		private void Awake()
		{
			if (GameMaster.Instance != null) return;

			Initialize(_soundManager);
		}

		private void Update()
		{
			Crouch();
		}

		private void FixedUpdate()
		{
			UpdateDistanceToGround();

			Move(Time.fixedDeltaTime);
			ApplyGravity(Time.fixedDeltaTime);
			Slide(Time.fixedDeltaTime);

			UpdatePlayerLayer();
		}

		private float _timer = 0f;
        private void PlayFootStep(float soundPower)
        {
			if(_soundManager != null)
            {
				var period = _curve.Evaluate(soundPower);
				_timer += Time.deltaTime;
				if(_timer >= period && period != 0 && period <= 1)
				{
					_timer = 0f;
					_soundManager.PlayAudio(_footIndex);
				}
			}
        }

        private void Crouch()
		{
			var isCrouching = _playerInputActions.Player.Crouch.IsPressed();
			if (isCrouching)
			{
				_currentHeight = _crouchingHeight;
			}
			else
			{
				_currentHeight = _playerHeight;
				_currentSpeed = _moveSpeed;
			}
		}

		private void UpdatePlayerLayer()
		{
			if (_distanceToGround < _crouchingHeight + 0.1f)
			{
				gameObject.layer = LayerMask.NameToLayer("PlayerStealth");
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer("PlayerDefault");
			}
		}

		private void ApplyGravity(float deltaTime)
		{
			var velocity = _rigidbody.velocity;
		
			if (IsTouchingGround)
			{
				var mod = velocity.y < 0 ? -velocity.y : 0f;
				var resultForce = mod * _gravityCompensation + _standForce * (1f - _distanceToGround / _currentHeight) * deltaTime;
			
				velocity.y += resultForce;
			}
			else
			{
				velocity += Vector3.down * _gravityForce * deltaTime;
			}

			_rigidbody.velocity = velocity;
		}

		private void Slide(float deltaTime)
		{
			_isSliding = false;
			if (Physics.Raycast(transform.position, Vector3.down, out var hit, _currentHeight))
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
			if (Physics.Raycast(transform.position, Vector3.down, out var hit))
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
				var speed = _distanceToGround < _playerHeight ? _distanceToGround / _playerHeight * _currentSpeed : _currentSpeed;
				var newVelocity = (characterForwardDir * moveInput.y + _head.right * moveInput.x).normalized * speed;

				horizontalVelocity = Vector3.Lerp(horizontalVelocity, newVelocity, _stopSpeed);

				var soundPower = _distanceToGround <= _playerHeight ? (_distanceToGround / _playerHeight) - 0.1f : 0f;
				PlayFootStep(soundPower);
			}

			horizontalVelocity.y = currentVelocity.y;
			currentVelocity = horizontalVelocity;

			_rigidbody.velocity = currentVelocity;
		}

		private void Jump(InputAction.CallbackContext obj)
		{
			if (_distanceToGround <= _currentHeight + 0.1f)
			{
				_rigidbody.velocity += (Vector3.up + _rigidbody.velocity.normalized).normalized * _jumpForce;

				_soundManager?.PlayAudio(_jumpIndex);
			}
		}

		private void Interact(InputAction.CallbackContext obj)
		{
			Debug.DrawRay(_head.position, (_head.forward * _interactDistance), Color.red);
			if (Physics.Raycast(_head.position, _head.forward, out var hit, _interactDistance))
			{
				var interactable = hit.collider.GetComponent<IInteractable>();

				if (interactable != null)
				{
					interactable.Interact(this);
				}
			}
		}
	}
}