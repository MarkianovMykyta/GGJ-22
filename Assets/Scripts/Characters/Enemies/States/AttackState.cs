using System.Collections;
using Contexts;
using UnityEngine;

namespace Characters.Enemies.States
{
	public class AttackState : State
	{
		private IDamageable _target;
		private Enemy _enemy;

		public AttackState(Enemy character, StateMachine stateMachine, Context context, IDamageable target) : base(character, stateMachine, context)
		{
			_enemy = character;
			character.CurrentState = EnemyState.Attack;
			_target = target;
		}

		public override void Enter()
		{
			_enemy.navMeshAgent.isStopped = true;

			_enemy.StartCoroutine(Attack());
		}

		private IEnumerator Attack()
		{
			Vector3 originalPosition = _enemy.View.position;
			Vector3 attackPosition = _target.View.position - _enemy.View.forward * 0.5f;
			attackPosition.y = 0f;

			float percent = 0;

			while (percent <= 1)
			{
				percent += Time.deltaTime * _enemy.AttackSpeed;
				float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
				_enemy.View.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

				yield return null;
			}
			
			ApplyDamageToTarget();

			_enemy.navMeshAgent.isStopped = false;
			_enemy.View.position = originalPosition;

			yield return new WaitForSeconds(1f);
			stateMachine.QuitFromOverlap();
		}

		private void ApplyDamageToTarget()
		{
			if (Vector3.Distance(_enemy.View.position, _target.View.position) < _enemy.AttackRange)
			{
				_target.ApplyDamage(_enemy.AttackDamage);
			}
		}

		public override void Update()
		{
		}
	}
}