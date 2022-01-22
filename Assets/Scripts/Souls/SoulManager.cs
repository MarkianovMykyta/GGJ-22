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

        private void Awake()
        {
            _poolView = new Stack<SoulView>(_soulViews.Count);
            for (int i = 0; i < _soulViews.Count; i++)
            {
                _poolView.Push(_soulViews[i]);
            }
        }

        public void Push(SoulView soulView)
        {
            _poolView.Push(soulView);
        }

        public SoulView Pop()
        {
            if(_poolView.TryPop(out SoulView view))
            {
                return view;
            }
            var newSoul = Instantiate(_soulViews[0]);
            newSoul.Deactivate();

            _soulViews.Add(newSoul);
            return newSoul;
        }
    }
}