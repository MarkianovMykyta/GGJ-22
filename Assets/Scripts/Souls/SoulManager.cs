using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class SoulManager : MonoBehaviour
    {
        [SerializeField] private List<SoulView> _soulViews;
        private Stack<SoulView> _poolView;

        private List<SoulView> _activeSouls;

        public SoulView SoulView()
        {
            if(_activeSouls.Count > 0)
            {
                return _activeSouls[_activeSouls.Count - 1];
            }

            return null;
        }

        public void MoveActiveSouls(Transform target, float radius)
        {
            for (int i = 0; i < _activeSouls.Count; i++)
            {
                var deltaTarget = target.position - _activeSouls[i].transform.position;
                if (deltaTarget.magnitude < radius)
                {
                    Debug.Log(deltaTarget.magnitude);

                    //_activeSouls[i].transform.Translate(deltaTarget.normalized * 10f * Time.deltaTime);
                    //Debug.Log(deltaTarget);
                    _activeSouls[i].StepMoveToTarget(deltaTarget.normalized);
                }
                else
                {
                    _activeSouls[i].SetUnActive();
                    //_activeSouls[i].SetKinematic(false);
                }
            }
        }

        public void StopActiveSouls()
        {
            for (int i = 0; i < _activeSouls.Count; i++)
            {
                _activeSouls[i].SetKinematic(true);
            }
        }

        private void Awake()
        {
            _activeSouls = new List<SoulView>();
            _poolView = new Stack<SoulView>(_soulViews.Count);
            for (int i = 0; i < _soulViews.Count; i++)
            {
                _poolView.Push(_soulViews[i]);
            }
        }

        public void Push(SoulView soulView)
        {
            _poolView.Push(soulView);
            _activeSouls.Remove(soulView);
        }

        public SoulView Pop()
        {
            if(_poolView.TryPop(out SoulView view))
            {
                _activeSouls.Add(view);
                return view;
            }
            var newSoul = Instantiate(_soulViews[0]);
            newSoul.Deactivate();

            _soulViews.Add(newSoul);
            _activeSouls.Add(newSoul);
            return newSoul;
        }
    }
}