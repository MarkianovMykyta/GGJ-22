using Characters.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InventoryBar : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private List<ItemView> _itemViews;
        [Space]
        [SerializeField] private CanvasGroup _canvasGroup;

        private List<ItemView> _openView;

        private ItemView _currentView;

        private void Awake()
        {
            for (int i = 0; i < _itemViews.Count; i++)
            {
                _itemViews[i].gameObject.SetActive(false);
            }

            _inventory.UpdatedItems += UpdatedItems;
            _inventory.ChangedIndex += OnChangedIndex;

            _openView = new List<ItemView>(_itemViews.Count);
        }

        private void OnChangedIndex(int i)
        {
            StartCoroutine(ShowPanel());

            _currentView.SetActive(false);
            _currentView = _openView[i];
            _currentView.SetActive(true);
        }

        private float _timer = 0f;
        private IEnumerator ShowPanel()
        {
            //_canvasGroup.alpha = 0f;

            //while (true)
            //{
            //    _timer += Time.deltaTime;
            //    yield return null;

            //    _canvasGroup.alpha = _timer / 0.3f;
            //    if (_timer > 0.3f)
            //    {
            //        break;
            //    }
            //}
            _canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(3f);

            _timer = 0f;
            while (true)
            {
                _timer += Time.deltaTime;
                yield return null;
                _canvasGroup.alpha = 1f - (_timer / 2f);
                if (_timer > 2f)
                {
                    break;
                }
            }

            _canvasGroup.alpha = 0f;
        }

        private void UpdatedItems(List<HandableItem> handableItems)
        {
            _openView.Clear();

            for (int i = 0; i < handableItems.Count; i++)
            {
                for (int j = 0; j < _itemViews.Count; j++)
                {
                    if(_itemViews[j].ItemType.HasFlag(handableItems[i].ItemType))
                    {
                        _openView.Add(_itemViews[j]);
                        _itemViews[j].gameObject.SetActive(true);
                        break;
                    }
                }
            }

            _currentView = _openView[_inventory.CurrentIndex];
            _currentView.SetActive(true);
        }
    }
}