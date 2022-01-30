using UnityEngine;
using UnityEngine.Events;

namespace Environment.Intractable
{
    public class TriggerEvent : UnityEvent {}

    public class FloorButton : MonoBehaviour
    {
        public UnityEvent triggerEnterEvent;
        public UnityEvent triggerExitEvent;

        [SerializeField] Animator ButtonAnimator;

        private int pushId = Animator.StringToHash("Push");

        private void OnTriggerEnter(Collider other)
        {
            ButtonAnimator.SetBool(pushId, true);
            triggerEnterEvent.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            ButtonAnimator.SetBool(pushId, false);
            triggerExitEvent.Invoke();
        }
    }
}