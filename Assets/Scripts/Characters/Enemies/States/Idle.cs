using System.Collections;
using UnityEngine;

using Contexts;
using UnityEngine.InputSystem;

namespace Characters.Enemies.States
{
    public class IdleState : State
    {
        private Enemy _enemy;

        private int _idleID = Animator.StringToHash("Idle");

        public IdleState(Enemy enemy, StateMachine stateMachine, Context context) : base(enemy, stateMachine, context)
        {
            _enemy = enemy;
            enemy.CurrentState = EnemyState.Idle;
        }

        public override void Enter()
        {
            _enemy.Animator.SetBool(_idleID, true);
        }

        public override void Exit()
        {
            _enemy.Animator.SetBool(_idleID, false);
        }

        private float timer;
        public override void Update()
        {

            if (Keyboard.current.spaceKey.isPressed)
            {
                stateMachine.ChangeState(new PatrolState(_enemy, stateMachine, context));
            } 
            else if (Keyboard.current.sKey.isPressed)
            {
                stateMachine.ChangeState(new DeadState(_enemy, stateMachine, context));
            }
        }
    }
}