using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Preparing.checks
{
    public class OnCheckItemClickListener : MonoBehaviour
    {
        public string title;
        public ShopScrollList scrollList;

        public void OnCheckClick(int price)
        {
            var item = new PickedShopItem() {itemTitle = title, itemPrice = price};
            scrollList.otherList.AddItem(item);
        }
    }
}