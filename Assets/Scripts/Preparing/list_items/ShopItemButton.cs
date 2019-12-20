using System;
using System.Collections.Generic;
using GameStuff;
using MetaInfo;
using Preparing.lists;
using UnityEngine;
using UnityEngine.UI;

namespace Preparing.list_items
{
    public class ShopItemButton : MonoBehaviour
    {
        public Button button;
        public Text titleText;
        public Text priceText;

        private Card _card;
        private ShopScrollList _scrollList;

        private void Start()
        {
            button.onClick.AddListener(HandleClick);
        }

        public void Setup(Card card, ShopScrollList scrollList, List<Sprite> sprites)
        {
            _card = card;
            _scrollList = scrollList;
            titleText.text = _card.GetText();
            

            priceText.text = "" + _card.GetPrice();
            
            var cardImage = GetComponent<Image>();

            switch (card.GetCardType())
            {
                case CardType.GOLD:
                    cardImage.sprite = sprites[0];
                    titleText.color = new Color32(255, 232, 174, 255);
                    break;
                
                case CardType.SILVER:
                    cardImage.sprite = sprites[1];
                    titleText.color = new Color32(236, 232, 232, 255);
                    break;
                
                case CardType.NEUTRAL:
                    cardImage.sprite = sprites[2];
                    titleText.color = new Color32(255, 227, 208, 255);
                    break;
                
                default:
                    cardImage.sprite = sprites[2];
                    titleText.color = new Color32(255, 227, 208, 255);
                    break;
            }

            button.transform.localScale = new Vector2(0.7f, 0.7f);
        }

        public void HandleClick()
        {
            var item = new PickedShopItem()
                {id = _card.GetId(), itemType = "Card", itemTitle = _card.GetText(), itemPrice = _card.GetPrice()};
            _scrollList.otherList.AddItem(item);
        }
    }
}