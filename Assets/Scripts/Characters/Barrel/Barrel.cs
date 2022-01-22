using UnityEngine;

using Characters.Barrel.States;

namespace Characters.Barrel
{
    public class Barrel : Character
    {
        [SerializeField] private BarrelState _currentState;

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
            stateMachine.Initialize(new IdleState(this, stateMachine, context));
        }
    }
}