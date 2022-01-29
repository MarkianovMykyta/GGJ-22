using UnityEngine;
using UnityEngine.AI;

using Characters;
using System.Collections;
using System;

namespace Souls
{
    public class SoulView : MonoBehaviour
    {
        public event Action Started;
        public event Action Finished;

        [SerializeField] private SoulManager _soulManager;
        [SerializeField] private Rigidbody _rigidbody;
        //[SerializeField] private NavMeshAgent _navMeshAgent;
        [Space]
        [SerializeField] private ParticleSystem _vfx;

        public Soul Soul;

        public void Initialize(Transform target, Soul soul)
        {
            gameObject.SetActive(true);
            _vfx.Clear();
            _vfx.Play();

            transform.position = target.position;
            Soul = soul;

            StartCoroutine(MoveUp(target));
        }

        private IEnumerator MoveUp(Transform fromTarget)
        {
            while (true)
            {
                _rigidbody.AddForce(Vector3.up*0.5f);

                yield return null;

                if (transform.position.y - fromTarget.position.y > 2f)
                {
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.isKinematic = true;
                    break;
                }
            }
        }

        public void SetUnActive()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = false;
        }

        public void MoveToTarget(Vector3 toTarget)
        {
            StartCoroutine(MoveTo(toTarget));
        }

        private float flyTimer = 0f;
        private IEnumerator MoveTo(Vector3 toTarget)
        {
            flyTimer = 0f;
            while (true)
            {
                yield return null;

                var directionToTarget = toTarget - transform.position;
                StepMoveToTarget(directionToTarget.normalized);

                flyTimer += Time.deltaTime;
                if (flyTimer >= 15f)
                {
                    break;
                }
            }
        }

        public void StepMoveToTarget(Vector3 directionToTarget)
        {
            var offsetY = Vector3.up * UnityEngine.Random.Range(-1f, 1f) * 0.3f;

            directionToTarget += offsetY;

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(directionToTarget * 50f, ForceMode.Force);

            if(_rigidbody.velocity.magnitude > 5f)
                _rigidbody.velocity = _rigidbody.velocity.normalized * 5f;
        }

        public void SetKinematic(bool v)
        {
            _rigidbody.isKinematic = v;
        }

        public void CombackToParent(bool autoUpdate = false)
        {
            Finished += () => SetToCharacter(Soul.Parent);
            Finished += Deactivate;

            //MoveToTarget(_soul.Parent.transform, autoUpdate);
        }

        public void Deactivate()
        {
            Started = null;
            Finished = null;
            Soul = null;

            _soulManager.Push(this);

            transform.SetParent(_soulManager.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            gameObject.SetActive(false);
        }

        public void SetToCharacter(Character character)
        {
            character.Soul = Soul;
        }
    }
}