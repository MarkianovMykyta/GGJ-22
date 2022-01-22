using Contexts;

namespace Characters.Barrel.States
{
    public class IdleState : State
    {
        public IdleState(Barrel character, StateMachine stateMachine, Context context) : base(character, stateMachine, context)
        {
            character.CurrentState = BarrelState.Idle;
        }
    }
}