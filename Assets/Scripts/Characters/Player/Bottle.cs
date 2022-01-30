using Contexts;
using Souls;
using System.Collections;
using Characters.Barrel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class Bottle : MonoBehaviour
    {
		private static readonly int PushID = Animator.StringToHash("Push");
		private static readonly int PullID = Animator.StringToHash("Pull");

		[SerializeField] private int _damage;
		[SerializeField] private float _bottleRange;
		[SerializeField] private float _cooldownTime;
		[SerializeField] private Animator _animator;
        [SerializeField] private Camera _camera;
        [SerializeField] private Collider _collider;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _fullBottle;
        [SerializeField] private Sprite _emptyBottle;
        //[SerializeField] private Sprite _fullBottle;

        private float _lastAttackTime;

		private PlayerInputActions _playerInputActions;
		private Context _context;

        private Soul _currentSoul;

        public Soul CurrentSoul
        {
            get
            {
                _spriteRenderer.sprite = _emptyBottle;
                return _currentSoul;
            }
            set
            {
                _spriteRenderer.sprite = _fullBottle;
                _currentSoul = value;
            }
        }

        public void OnDrawGizmos()
        {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, _bottleRange);
        }

        private void Awake()
		{
			_playerInputActions = new PlayerInputActions();
			_playerInputActions.Bottle.Push.started += Push;
            _playerInputActions.Bottle.Pull.canceled += UnPull;

            _context = FindObjectOfType<Context>();
            UpdateView();
		}

		private void OnEnable()
		{
			_playerInputActions.Bottle.Enable();
		}

		private void OnDisable()
		{
			_playerInputActions.Bottle.Disable();
		}

        public void UpdateView()
        {
            _spriteRenderer.sprite = _currentSoul != null ? _fullBottle : _emptyBottle;
        }

        private void Push(InputAction.CallbackContext obj)
        {
            Debug.Log("Push Soul");

            if(_currentSoul != null)
            {
                Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _bottleRange, Color.red);
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit, _bottleRange))
                {
                    var soulable = hit.collider.GetComponent<ISoulable>();
                    if (soulable != null)
                    {
                        Debug.Log($"Hit {hit.collider.gameObject}");

                        StartCoroutine(ActivatePushBlocker());
                        var soulView = _context.SoulManager.Pop();
                        //soulView.transform.position = transform.position;
                        soulView.Initialize(transform, _currentSoul);
                        soulView.MoveToTarget(hit.point);

                        _currentSoul = null;
                        UpdateView();
                    }
                }
            }
        }

        private IEnumerator ActivatePushBlocker()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(2f);
            _collider.enabled = true;
        }

        //private void Pull(InputAction.CallbackContext obj)
        //      {
        //	Debug.Log("Pull Soul");

        //	_context.SoulManager.MoveActiveSouls(transform, _bottleRange);
        //      }

        private void UnPull(InputAction.CallbackContext obj)
        {
            _context.SoulManager.StopActiveSouls();
        }

        public void Update()
        {
            if (_playerInputActions.Bottle.Pull.IsPressed() && _currentSoul == null)
            {
                _context.SoulManager.MoveActiveSouls(transform, _bottleRange);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out SoulView soulView))
            {
                _currentSoul = soulView.Soul;

                _context.SoulManager.StopActiveSouls();
                soulView.SetUnActive();
                soulView.Deactivate();
                _context.SoulManager.Push(soulView);

                UpdateView();
            }
        }
    }
}