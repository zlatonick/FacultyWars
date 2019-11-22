using System;
using UnityEngine;
using UnityEngine.UI;

namespace Preparing.list_items
{
    public class ShopItemButton : MonoBehaviour
    {
        public Button button;
        public Text titleText;

        private ShopItem _item;
        private ShopScrollList _scrollList;

        private void Start()
        {
            button.onClick.AddListener(HandleClick);
        }

        public void Setup(ShopItem item, ShopScrollList scrollList)
        {
            _item = item;
            _scrollList = scrollList;
            titleText.text = _item.itemTitle;
        }

        public void HandleClick()
        {
            var item = new PickedShopItem() {itemTitle = _item.itemTitle, itemPrice = 5};
            _scrollList.otherList.AddItem(item);
        }
    }
}