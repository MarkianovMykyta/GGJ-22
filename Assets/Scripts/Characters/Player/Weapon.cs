using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
	public class Weapon : MonoBehaviour
	{
		private static readonly int Attack1 = Animator.StringToHash("Attack");
		
		[SerializeField] private int _damage;
		[SerializeField] private float _attackRange;
		[SerializeField] private float _cooldownTime;
		[SerializeField] private Animator _animator;

		private float _lastAttackTime;


		private PlayerInputActions _playerInputActions;

		private void Awake()
		{
			_playerInputActions = new PlayerInputActions();
			_playerInputActions.Player.Attack.performed += Attack;
		}

		private void OnEnable()
		{
			_playerInputActions.Player.Enable();
		}

		private void OnDisable()
		{
			_playerInputActions.Player.Disable();
		}

		private void Attack(InputAction.CallbackContext obj)
		{
			if(Time.time - _lastAttackTime < _cooldownTime) return;

			_lastAttackTime = Time.time;
			
			Debug.DrawRay(transform.position, transform.forward * _attackRange, Color.red);
			if (Physics.Raycast(transform.position, transform.forward, out var hit, _attackRange))
			{
				var damageable = hit.collider.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.ApplyDamage(_damage);
					Debug.LogError("Attacked!");
				}
			}

			if (_animator != null)
			{
				_animator.SetTrigger(Attack1);
			}
		}
	}
}