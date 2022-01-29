using UnityEngine;
using UnityEngine.AI;

using Contexts;
using Souls;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Character : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _health;
        
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
        public Transform View { get; protected set; }
        public int Health => _health;
        public bool IsDestroyed { get; private set; }
        
        public void ApplyDamage(int damage)
        {
            if(IsDestroyed) return;
            
            _health -= damage;

            if (_health <= 0)
            {
                _health = 0;
                Die();
            }
        }
        
        private void Awake()
        {
            if (context == null) context = FindObjectOfType<Context>();

            stateMachine = new StateMachine();
            soul = new Soul(this);

            View = transform;
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

        protected virtual void Die()
        {
            if(IsDestroyed) return;
            IsDestroyed = true;
        }
    }
}