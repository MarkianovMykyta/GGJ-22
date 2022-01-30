using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.Crowd
{
	public class Silhouette : MonoBehaviour
	{
		public event Action<Silhouette> TravelComplete;

		[SerializeField] private float _maxMoveSpeed;
		
		private float _moveSpeed;
		private Vector3 _moveDirection;
		private Vector3 _startPoint;
		private float _travelDistance;

		private Animator _animator;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public void Init(float moveSpeed, Vector3 moveDirection, Vector3 startPoint, float travelDistance)
		{
			transform.position = startPoint;
			
			_moveSpeed = moveSpeed;
			_moveDirection = moveDirection;
			_startPoint = startPoint;
			_travelDistance = travelDistance;
			
			transform.Rotate(Vector3.up, Random.Range(0, 360), Space.Self);

			_animator.speed = moveSpeed / _maxMoveSpeed;
		}
		
		private void Update()
		{
			transform.position += _moveDirection * _moveSpeed * Time.deltaTime;

			if (Vector3.Distance(transform.position, _startPoint) > _travelDistance)
			{
				TravelComplete?.Invoke(this);
			}
		}
	}
}