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

            if (distanceToTarget < _enemy.AttackRange)
            {
                stateMachine.OverlapState(new AttackState(_enemy, stateMachine, context, _target));
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

    public class AttackState : State
    {
        private Transform _target;
        private Enemy _enemy;

        public AttackState(Enemy character, StateMachine stateMachine, Context context, Transform target) : base(character, stateMachine, context)
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
            Vector3 attackPosition = _target.position - _enemy.View.forward * 0.5f;
            attackPosition.y = 0f;

            float percent = 0;

            while (percent <= 1)
            {
                percent += Time.deltaTime * _enemy.AttackSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                _enemy.View.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

                yield return null;
            }

            _enemy.navMeshAgent.isStopped = false;
            _enemy.View.position = originalPosition;

            yield return new WaitForSeconds(1f);
            stateMachine.QuitFromOverlap();
        }

        public override void Update()
        {
        }
    }
}