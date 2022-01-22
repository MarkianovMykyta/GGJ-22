using Contexts;
using UnityEngine.InputSystem;

namespace Characters.Barrel.States
{
    public class IdleState : State
    {
        private Barrel _barrel;

        public IdleState(Barrel barrel, StateMachine stateMachine, Context context) : base(barrel, stateMachine, context)
        {
            _barrel = barrel;

            barrel.CurrentState = BarrelState.Idle;
        }

        public override void Update()
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                stateMachine.ChangeState(new ChaseState(_barrel, stateMachine, context));
            }
        }
    }
}