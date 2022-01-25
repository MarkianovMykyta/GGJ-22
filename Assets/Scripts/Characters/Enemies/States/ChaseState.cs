using Contexts;
using System;
using System.Collections;
using UnityEngine;

namespace Characters.Enemies.States
{
    public class ChaseState : PatrolState
    {
        private Transform _target;
        private Coroutine _chaseCheckerRoutine;

        public ChaseState(Enemy character, StateMachine stateMachine, Context context, Transform target) : base(character, stateMachine, context)
        {
            character.CurrentState = EnemyState.Chase;
            _target = target;
        }

        public override void Enter()
        {
            _chaseCheckerRoutine = _enemy.StartCoroutine(DistanceCheckerRoutine());
            _enemy.navMeshAgent.destination = _target.position;
            _enemy.Animator.SetBool(_walkId, true);
        }

        public override void Exit()
        {
            _enemy.Animator.SetBool(_walkId, false);
            _enemy.StopCoroutine(_chaseCheckerRoutine);
        }

        private IEnumerator DistanceCheckerRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while (true)
            {
                yield return wait;
                ChaseToTarget();
            }
        }

        private void ChaseToTarget()
        {
            float distanceToTarget = Vector3.Distance(_enemy.transform.position, _target.position);

            var damageableComponent = _target.GetComponent<IDamageable>();
            if (damageableComponent != null && distanceToTarget < _enemy.AttackRange)
            {
                stateMachine.OverlapState(new AttackState(_enemy, stateMachine, context, damageableComponent));
            }
            else if(distanceToTarget >= _enemy.DetectionRadius)
            {
                stateMachine.ChangeState(new LocalInspection(_enemy, stateMachine, context));
            } 
            else
            {
                if (Physics.Raycast(_enemy.transform.position, _target.position - _enemy.transform.position, distanceToTarget, _enemy.ObstructionMask))
                {
                    stateMachine.ChangeState(new LocalInspection(_enemy, stateMachine, context));
                    return;
                }

                _enemy.navMeshAgent.destination = _target.transform.position;
            }
        }

        public override void Update()
        {
        }
    }
}