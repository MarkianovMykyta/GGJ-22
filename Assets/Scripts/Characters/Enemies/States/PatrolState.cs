using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using Contexts;
using Enviroment;

namespace Characters.Enemies.States
{
    public class PatrolState : State
    {
        private List<Waypoint> _waypoints;
        private NavMeshAgent _navMeshAgent;

        private int _currentWaypointIndex;
        private Vector3 _targetPosition;
        private Enemy _enemy;
        public PatrolState(Enemy enemy, StateMachine stateMachine, Context context) : base(enemy, stateMachine, context)
        {
            _currentWaypointIndex = 0;
            enemy.CurrentState = EnemyState.Patrol;

            _enemy = enemy;
        }

        public override void Enter()
        {
            _waypoints = context.WaypointManager.Waypoints;
            _navMeshAgent = _enemy.navMeshAgent;
            _navMeshAgent.destination = _waypoints[_currentWaypointIndex].transform.position;
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