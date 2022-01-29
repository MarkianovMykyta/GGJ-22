
using Contexts;
using UnityEngine;

namespace Characters.Enemies.States
{
    public class DeadState : State
    {
        private Enemy _enemy;

        public DeadState(Enemy enemy, StateMachine stateMachine, Context context) : base(enemy, stateMachine, context)
        {
            _enemy = enemy;
            enemy.CurrentState = EnemyState.Die;
        }

        public override void Enter()
        {
            _enemy.Rigidbody.isKinematic = false;
            _enemy.Rigidbody.useGravity = true;
            _enemy.navMeshAgent.enabled = false;
            _enemy.Animator.enabled = false;

            var randomCircle = Random.insideUnitSphere;
            randomCircle.y = 0f;
            randomCircle.Normalize();

            _enemy.Rigidbody.AddForce(randomCircle, ForceMode.Impulse);

            var soulViewer = context.SoulManager.Pop();
            soulViewer.Initialize(_enemy.gameObject.transform, _enemy.Soul);
        }
    }
}