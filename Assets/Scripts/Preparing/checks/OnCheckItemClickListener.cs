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

            switch (type)
            {
                case StuffClass.IASA:
                {
                    image.sprite = spriteIasa;
                    _shopCheck = PreparingData.shopChecksIasa[index];
                    break;
                }
                
                case StuffClass.FICT:
                {
                    image.sprite = spriteFict;
                    _shopCheck = PreparingData.shopChecksFict[index];
                    break;
                }
                
                case StuffClass.FPM:
                {
                    image.sprite = spriteFpm;
                    _shopCheck = PreparingData.shopChecksFpm[index];
                    break;
                }
            }
            
            GetComponentInChildren<Text>().text = "" + _shopCheck.power;
            this.gameObject.transform.parent.GetComponentsInChildren<Text>()[1].text = _shopCheck.price + "$";
        }

        public void OnCheckClick()
        {
            var item = new PickedShopItem() { level = index, itemType = "Check", itemTitle = "+" + _shopCheck.power + " power", itemPrice = _shopCheck.price};
            scrollList.AddItem(item);
        }
    }
}