using UnityEngine;
using UnityEngine.AI;

using Contexts;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected Context context;
        public NavMeshAgent navMeshAgent;
        protected StateMachine stateMachine;
        protected Soul soul;
        public Soul Soul
        {
            get
            {
                return soul;
            }
            set
            {
                soul = value;
            }
        }

        public bool HasSoul => soul != null;

        private void Awake()
        {
            if (context == null) context = FindObjectOfType<Context>();

            stateMachine = new StateMachine();
        }

        private void Update()
        {
            UpdateLogic();
        }

        private void FixedUpdate() => FixedUpdateLogic();

        protected virtual void UpdateLogic()
        {
            stateMachine.Update();
        }

        protected virtual void FixedUpdateLogic()
        {
            stateMachine.FixedUpdate();
        }
    }
}