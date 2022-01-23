using Contexts;
using System.Collections;

namespace Characters.Barrel.States
{
    public class ChaseState : State
    {
        private Barrel _barrel;

        public ChaseState(Barrel barrel, StateMachine stateMachine, Context context) : base(barrel, stateMachine, context)
        {
            _barrel = barrel;
        }

        public override void Enter()
        {
            context.StartCoroutine(UpdateDestination());
        }

        private IEnumerator UpdateDestination()
        {
            while (true)
            {
                yield return new UnityEngine.WaitForSeconds(0.5f);

                _barrel.navMeshAgent.destination = context.Target.transform.position;
            }
        }

        public override void Update()
        {
        }
    }
}