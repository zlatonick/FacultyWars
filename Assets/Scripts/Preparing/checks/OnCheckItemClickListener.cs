using System;
using MetaInfo;
using Preparing.lists;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Preparing.checks
{
    public class OnCheckItemClickListener : MonoBehaviour
    {
        public int index;
        public PickedShopScrollList scrollList;
        public Sprite spriteIasa;
        public Sprite spriteFict;
        public Sprite spriteFpm;

        private ShopCheck _shopCheck;

        private void Start()
        {
            var image = GetComponent<Image>();
            var type = StuffPack.stuffClass;
            var priceText = GetComponentInChildren<Text>();
            
            switch (type)
            {
                case StuffClass.IASA:
                {
                    image.sprite = spriteIasa;
                    _shopCheck = PreparingData.shopChecksIasa[index];
                    priceText.color = new Color32(142, 47, 5, 255);
                    break;
                }
                
                case StuffClass.FICT:
                {
                    image.sprite = spriteFict;
                    _shopCheck = PreparingData.shopChecksFict[index];
                    priceText.color = new Color32(76, 111, 13, 255);
                    break;
                }
                
                case StuffClass.FPM:
                {
                    image.sprite = spriteFpm;
                    _shopCheck = PreparingData.shopChecksFpm[index];
                    priceText.color = new Color32(45, 70, 111, 255);
                    break;
                }
            }
            
            priceText.text = "" + _shopCheck.power;
            
            this.gameObject.transform.parent.GetComponentsInChildren<Text>()[1].text = _shopCheck.price + "";
        }

        public void OnCheckClick()
        {
            var item = new PickedShopItem() { level = index, itemType = "Check", itemTitle = "+" + _shopCheck.power + " power", itemPrice = _shopCheck.price};
            scrollList.AddItem(item);
        }
    }
}