using System.Collections;
using UnityEngine;

using Contexts;
using UnityEngine.InputSystem;

namespace Characters.Enemies.States
{
    public class IdleState : State
    {
        private Enemy _enemy;

        public IdleState(Enemy enemy, StateMachine stateMachine, Context context) : base(enemy, stateMachine, context)
        {
            _enemy = enemy;
            enemy.CurrentState = EnemyState.Idle;
        }
        private float timer;
        public override void Update()
        {
            /*timer += Time.deltaTime;
            if (timer > 10f)
            {
            }*/

            if (Keyboard.current.spaceKey.isPressed)
            {
                stateMachine.ChangeState(new PatrolState(_enemy, stateMachine, context));
            }
        }
    }
}