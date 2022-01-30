using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Characters.Player
{
    public class Inventory : MonoBehaviour
    {
        public Action<List<HandableItem>> UpdatedItems;
        public Action<int> ChangedIndex;

        [SerializeField] private List<HandableItem> _handableItems;

        private ItemType _items;

        private HandableItem _currentHandableItem;
        private PlayerInputActions _playerInputActions;

        private List<HandableItem> _openItems;

        public int _currentIndex;

        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                //_currentIndex = (int)Mathf.Clamp(value, 0, _openItems.Count-1);

                _currentIndex = value;

                if (_currentIndex < 0)
                {
                    _currentIndex = _openItems.Count - 1;
                }
                else if(_currentIndex > _openItems.Count - 1)
                {
                    _currentIndex = 0;
                }

                SwapItem();
                ChangedIndex?.Invoke(_currentIndex);
            }
        }

        private void SwapItem()
        {
            _currentHandableItem.gameObject.SetActive(false);
            _currentHandableItem = _openItems[_currentIndex];
            _currentHandableItem.gameObject.SetActive(true);
        }

        public void Initialize(ItemType items)
        {
            _items = items;

            for (int i = 0; i < _handableItems.Count; i++)
            {
                _handableItems[i].gameObject.SetActive(false);
            }

            _openItems = new List<HandableItem>(_handableItems.Count);
            DefineOpenItems(items);

            _currentHandableItem = _openItems[0];
            CurrentIndex = 0;
        }

        private void DefineOpenItems(ItemType items)
        {
            foreach (ItemType flagToCheck in Enum.GetValues(typeof(ItemType)))
            {
                if (items.HasFlag(flagToCheck))
                {
                    for (int j = 0; j < _handableItems.Count; j++)
                    {
                        if (_handableItems[j].ItemType == flagToCheck)
                        {
                            _openItems.Add(_handableItems[j]);
                        }
                    }
                }
            }

            UpdatedItems?.Invoke(_openItems);
        }

        private void Awake()
        {
            Initialize(ItemType.None | ItemType.Bottle | ItemType.Dagger);

            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _playerInputActions.Inventory.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Inventory.Disable();
        }

        private void Scroll(int value)
        {
            CurrentIndex -= value;
            Debug.Log($"Current scrollIndex = {CurrentIndex}");
        }

        private void Update()
        {
            float value = _playerInputActions.Inventory.Scroll.ReadValue<float>();
            
            if(value != 0)
            {
                int direction =
                    value > 0 ? 1 :
                    value < 0 ? -1 : 0;

                Scroll(direction);
            }
        }
    }
}