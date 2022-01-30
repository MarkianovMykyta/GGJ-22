using Characters.Barrel.States;
using Souls;
using UnityEngine;

namespace Characters.Barrel
{
    public interface ISoulable
    {
        void SetSoul(Soul soul);
        bool HasSoul();
    }

    public class Barrel : Character, ISoulable
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

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent(out SoulView soulView))
            {
                SetSoul(soulView.Soul);
                soulView.Deactivate();
            }
        }

        public void SetSoul(Soul soul)
        {
            Soul = soul;

            stateMachine.ChangeState(new ChaseState(this, stateMachine, context));
        }

        bool ISoulable.HasSoul()
        {
            return Soul != null;
        }
    }
}