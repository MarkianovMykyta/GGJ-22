using Characters;
using UnityEngine;

using Characters.Enemies.States;

namespace Characters.Enemies
{
    public class Enemy : Character
    {
        [SerializeField] private Animator _animator;

        [Header("State")]
        public EnemyState CurrentState;

        [Header("Configuration")]
        public float DetectionRadius;
        public float Angle;
        public LayerMask TargetMask;
        public LayerMask ObstructionMask;
        [Space] 
        public int AttackDamage;
        public float AttackRange;
        public float AttackSpeed;

        public Animator Animator => _animator;

        private void Start()
        {
            stateMachine.ChangeState(new IdleState(this, stateMachine, context));
        }

        private void OnDrawGizmos()
        {
            if (View == null)
            {
                View = transform;
            }
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DetectionRadius);

            Vector3 viewAngle01 = DirectionFromAngle(View.eulerAngles.y, -Angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(View.eulerAngles.y, Angle / 2);

            Gizmos.color = Color.red;

            Gizmos.DrawLine(View.position, View.position + viewAngle01 * DetectionRadius);
            Gizmos.DrawLine(View.position, View.position + viewAngle02 * DetectionRadius);
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}