using Contexts;

namespace Characters.Barrel.States
{
    public class ChaseState : State
    {
        public ChaseState(Barrel character, StateMachine stateMachine, Context context) : base(character, stateMachine, context)
        {
        }
    }
}