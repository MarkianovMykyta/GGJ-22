using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Image _backImage;

        public ItemType ItemType => _itemType;

        public void SetActive(bool value)
        {
            if (value)
            {
                _backImage.color = Color.gray;
            }
            else
            {
                _backImage.color = new Color(0, 0, 0, 0);
            }
        }
    }
}