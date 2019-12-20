using MetaInfo;
using UnityEngine;
using UnityEngine.UI;

namespace Preparing.list_items
{
    public class PickedShopItemButton : MonoBehaviour
    {
        public Button button;
        public Text titleLabel;
        public Text priceLabel;

        private PickedShopItem _item;
        private PickedShopScrollList _scrollList;

        void Start()
        {
            button.onClick.AddListener(HandleClick);
        }

        public void Setup(PickedShopItem item, PickedShopScrollList scrollList)
        {
            _item = item;
            titleLabel.text = item.itemTitle;
            priceLabel.text = "" + item.itemPrice;

            _scrollList = scrollList;

            button.transform.localScale = new Vector2(1, 1);
        }

        public void HandleClick()
        {
            _scrollList.RemoveItem(_item);
        }
        
    }
}