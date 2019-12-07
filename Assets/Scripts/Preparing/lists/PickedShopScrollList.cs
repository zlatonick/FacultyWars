using System;
using System.Collections.Generic;
using Preparing.list_items;
using Preparing.pools;
using UnityEngine;
using UnityEngine.UI;

namespace Preparing
{
    [Serializable]
    public class PickedShopItem
    {
        // 'Check' or 'Card'
        public int id = -1;
        public int level = -1;
        public string itemType;
        public string itemTitle;
        public int itemPrice;
    }

    public class PickedShopScrollList : MonoBehaviour
    {
        //TODO: remove
        public List<PickedShopItem> itemList;

        public Transform contentPanel;

        public Text moneyAmountText;
        public ShopPickedItemsPool itemsPool;

        //TODO: remove
        public int money = 1000;

        // Start is called before the first frame update
        private void Start() { RefreshDisplay(); }

        private void RefreshDisplay()
        {
            moneyAmountText.text = "Total money: " + money;
            RemoveButtons();
            AddButtons();
        }

        private void AddButtons()
        {
            foreach (var item in itemList)
            {
                var itemButton = itemsPool.GetObject();
                itemButton.transform.SetParent(contentPanel);

                var shopItemButton = itemButton.GetComponent<PickedShopItemButton>();
                shopItemButton.Setup(item, this);
            }
        }

        private void RemoveButtons()
        {
            while (contentPanel.childCount > 0)
            {
                GameObject toRemove = transform.GetChild(0).gameObject;
                itemsPool.ReturnObject(toRemove);
            }
        }
        
        public void AddItem(PickedShopItem item)
        {
            if (money < item.itemPrice)
                return; 
            
            itemList.Add(item);
            money -= item.itemPrice;
            RefreshDisplay();
        }

        public void RemoveItem(PickedShopItem itemToRemove)
        {
            for (var i = itemList.Count - 1; i >= 0; i--)
            {
                if (itemList[i] == itemToRemove)
                {
                    money += itemList[i].itemPrice;
                    itemList.RemoveAt(i);
                }
            }
            
            RefreshDisplay();
        }
    }
}