using System;
using UnityEngine;

namespace Characters.Player
{
	public class Player : MonoBehaviour, IDamageable
	{
		[SerializeField] private int _health;

		public int Health => _health;
		public Transform View { get; private set; }
		public bool IsDestroyed { get; private set; }

		private void Awake()
		{
			View = transform;
		}

		public void ApplyDamage(int damage)
		{
			if(IsDestroyed) return;
			
			_health -= damage;

			if (_health <= 0)
			{
				_health = 0;
				Die();
			}
		}

		private void Die()
		{
			if(IsDestroyed) return;

			IsDestroyed = true;
			Destroy(gameObject, 1f);
		}
	}
}