using UnityEngine;
using UnityEngine.AI;

using Characters;
using System.Collections;
using System;

namespace Souls
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SoulView : MonoBehaviour
    {
        public event Action Started;
        public event Action Finished;

        [SerializeField] private SoulManager _soulManager;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [Space]
        [SerializeField] private ParticleSystem _vfx;

        private Soul _soul;

        public void Initialize(Transform target, Soul soul)
        {
            gameObject.SetActive(true);
            _vfx.Clear();
            _vfx.Play();

            transform.position = target.position;
            _soul = soul;
        }

        public void MoveToTarget(Transform target, bool autoUpdate = false)
        {
            Started?.Invoke();

            if (autoUpdate)
            {
                StartCoroutine(UpdateDestination(target));
            }
            else
            {
                _navMeshAgent.destination = transform.position;
            }

            StartCoroutine(CheckDistanceAndEnter());
        }

        public void CombackToParent(bool autoUpdate = false)
        {
            Finished += () => SetToCharacter(_soul.Parent);
            Finished += Deactivate;

            MoveToTarget(_soul.Parent.transform, autoUpdate);
        }

        public void Deactivate()
        {
            Started = null;
            Finished = null;
            _soul = null;

            _soulManager.Push(this);

            transform.SetParent(_soulManager.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            gameObject.SetActive(false);
        }

        public void SetToCharacter(Character character)
        {
            character.Soul = _soul;
        }

        private IEnumerator UpdateDestination(Transform target)
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                _navMeshAgent.destination = transform.position;
            }
        }

        private IEnumerator CheckDistanceAndEnter()
        {
            while (true)
            {
                if(Vector3.Distance(transform.position, _navMeshAgent.destination) <= 1f)
                {
                    Finished?.Invoke();
                    break;
                }
            }

            yield return null;
        }
    }
}