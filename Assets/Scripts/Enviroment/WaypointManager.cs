using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enviroment
{
    [ExecuteInEditMode]
    public class WaypointManager : MonoBehaviour
    {
        public List<Waypoint> Waypoints => _waypoints;

        [SerializeField] private List<Waypoint> _waypoints;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < _waypoints.Count - 1; i++)
            {
                Gizmos.color = Color.HSVToRGB((i + 1f) / _waypoints.Count, 1, 1);
                Gizmos.DrawLine(_waypoints[i].transform.position, _waypoints[i + 1].transform.position);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_waypoints[_waypoints.Count - 1].transform.position, _waypoints[0].transform.position);
        }

        private void Update()
        {
            _waypoints = GetComponentsInChildren<Waypoint>().ToList();
        }
#endif
    }
}