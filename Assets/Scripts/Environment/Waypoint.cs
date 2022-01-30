using UnityEngine;

namespace Environment
{
    public class Waypoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}