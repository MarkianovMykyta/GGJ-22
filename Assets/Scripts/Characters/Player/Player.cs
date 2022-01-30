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

		private int _hitIndex;

		private SoundManager _soundManager;
		public void Initialize(SoundManager soundManager)
        {
			_soundManager = soundManager;
			_hitIndex = _soundManager.GetAudioClipIndex("hit");
		}

		private void Awake()
		{
			View = transform;
		}

		public void ApplyDamage(int damage)
		{
			if(IsDestroyed) return;
			
			_health -= damage;

			_soundManager?.PlayAudio(_hitIndex);

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
			gameObject.SetActive(false);
		}
	}
}