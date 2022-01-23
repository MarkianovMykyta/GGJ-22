using System.Collections;
using UnityEngine;

namespace Enviroment.Intractable
{
    public class CubeDoor : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private int openID = Animator.StringToHash("Open");

        public void Open()
        {
            _animator.SetBool(openID, true);
        }

        public void Close()
        {
            _animator.SetBool(openID, false);
        }
    }
}