using System.Collections;
using UnityEngine;

namespace Characters.Player
{
    public class EmptyHand : HandableItem
    {
        private const ItemType _itemType = ItemType.None;

        public override ItemType ItemType { get => _itemType; }
    }
}