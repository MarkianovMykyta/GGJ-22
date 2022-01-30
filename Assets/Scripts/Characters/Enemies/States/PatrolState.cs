using Contexts;
using System.Collections;
using System.Collections.Generic;
using Environment;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemies.States
{
    public class PatrolState : State
    {
        private List<Waypoint> _waypoints;
        private NavMeshAgent _navMeshAgent;

        private int _currentWaypointIndex;
        protected Enemy _enemy;

        private Coroutine _fovRoutine;

        protected int _walkId = Animator.StringToHash("Walk");

        public PatrolState(Enemy enemy, StateMachine stateMachine, Context context) : base(enemy, stateMachine, context)
        {
            _currentWaypointIndex = 0;
            enemy.CurrentState = EnemyState.Patrol;

            _enemy = enemy;
        }

        public override void Enter()
        {
            _waypoints = context.WaypointManager.GetWaypointsByCharacter(_enemy).Waypoints;
            
            _navMeshAgent = _enemy.navMeshAgent;
            _navMeshAgent.destination = FindNearestPoint(_enemy.transform.position,_waypoints);

            _enemy.Animator.SetBool(_walkId, true);

            _fovRoutine = _enemy.StartCoroutine(FOVRoutine());
        }

        protected IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        protected void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(_enemy.transform.position, _enemy.DetectionRadius, _enemy.TargetMask);

            if (rangeChecks.Length != 0)
            {
                Debug.Log(rangeChecks.Length);

                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - _enemy.View.transform.position).normalized;
                directionToTarget.y = 0f;

                if (Vector3.Angle(_enemy.View.transform.forward, directionToTarget) < _enemy.Angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(_enemy.transform.position, target.position);

                    //var obstructionMask = ~(1 << LayerMask.NameToLayer(_enemy.ObstructionMask.ToString())); 
                    if (!Physics.Raycast(_enemy.View.transform.position, directionToTarget, distanceToTarget, _enemy.ObstructionMask))
                    {
                        Debug.Log("Target finded");
                        stateMachine.ChangeState(new ChaseState(_enemy, stateMachine, context, target));
                    }
                }
            }
        }

        private Vector3 FindNearestPoint(Vector3 position, List<Waypoint> waypoints)
        {
            int indexBuffer = 0;
            float minDistance = Vector3.Distance(position, waypoints[0].transform.position);
            for (int i = 1; i < waypoints.Count; i++)
            {
                float distance = Vector3.Distance(position, waypoints[i].transform.position);
                if (distance < minDistance)
                {
                    indexBuffer = i;
                    minDistance = distance;
                }
            }

            _currentWaypointIndex = indexBuffer;
            return waypoints[indexBuffer].transform.position;
        }

        public override void Exit()
        {
            _enemy.Animator.SetBool(_walkId, false);
            _enemy.StopCoroutine(_fovRoutine);
        }

        public override void Update()
        {
            if(Vector3.Distance(_navMeshAgent.transform.position, _navMeshAgent.destination) < 1.0f)
            {
                NextWaypoint();
            }
        }

        private void NextWaypoint()
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= _waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }
            _navMeshAgent.destination = _waypoints[_currentWaypointIndex].transform.position;
        }
    }
}