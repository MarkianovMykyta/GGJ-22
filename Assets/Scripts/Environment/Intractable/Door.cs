using System;
using Characters.Player;
using UnityEngine;

namespace Environment.Intractable
{
    public class Door : MonoBehaviour, IInteractable
    {
        public event Action<LocationType> Opened;

        [SerializeField] private LocationType _nextLocation;
        //[SerializeField] private Animator _animator;
        [SerializeField] private Transform _spawnPosition;

        public void Interact(PlayerController playerController)
        {
            playerController.Freeze();

            if (_nextLocation != LocationType.None)
                Opened?.Invoke(_nextLocation);
            else
                throw new Exception("None Location");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "d_ViewToolMove On@2x");

            Gizmos.DrawIcon(_spawnPosition.position, "SoftlockProjectBrowser Icon");
        }
    }
}