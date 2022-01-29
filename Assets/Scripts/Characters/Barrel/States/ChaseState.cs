using Contexts;
using System.Collections;
using UnityEngine;

namespace Characters.Barrel.States
{
    public class ChaseState : State
    {
        private Barrel _barrel;
        private Transform _target;

        public ChaseState(Barrel barrel, StateMachine stateMachine, Context context) : base(barrel, stateMachine, context)
        {
            _barrel = barrel;
        }

        public override void Enter()
        {
            _target = context.Target.transform; //
            context.StartCoroutine(UpdateDestination());
        }

        private IEnumerator UpdateDestination()
        {
            while (true)
            {
                yield return new UnityEngine.WaitForSeconds(1f);

                _barrel.navMeshAgent.destination = _target.position;
            }
        }

        public override void Update()
        {
        }
    }
}