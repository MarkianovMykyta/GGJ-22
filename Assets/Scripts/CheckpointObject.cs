using System;
using UnityEngine;

public class CheckpointObject : MonoBehaviour
{
    public event Action Activate;

    public Transform SpawnTransform;

    private void OnTriggerEnter(Collider other)
    {
        Activate?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "d_SaveAs@2x");
    }
}
