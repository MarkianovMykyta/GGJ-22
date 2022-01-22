using Characters;
using UnityEngine;

using Characters.Enemies.States;

namespace Characters.Enemies
{
    public class Enemy : Character
    {
        public EnemyState CurrentState;

        private void Start()
        {

            stateMachine.ChangeState(new IdleState(this, stateMachine, context));
            //stateMachine.Initialize(_idle);
        }
    }
}