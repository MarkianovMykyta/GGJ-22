using UnityEngine;

using Characters.Barrel.States;

namespace Characters.Barrel
{
    public class Barrel : Character
    {
        [SerializeField] private BarrelState _currentState;

        private IdleState _idle;
        private ChaseState _chase;

        public BarrelState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
            }
        }


        private void Start()
        {
            _idle = new IdleState(this, stateMachine, context);
            _chase = new ChaseState(this, stateMachine, context);

            stateMachine.Initialize(_idle);
        }
    }
}