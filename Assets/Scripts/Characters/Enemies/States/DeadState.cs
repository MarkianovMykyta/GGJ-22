
using Contexts;

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
            var soulViewer = context.SoulManager.Pop();
            soulViewer.Initialize(_enemy.gameObject.transform, _enemy.Soul);
        }
    }
}