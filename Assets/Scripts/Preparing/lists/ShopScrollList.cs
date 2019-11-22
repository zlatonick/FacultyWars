using System;
using System.Collections.Generic;
using Preparing.list_items;
using Preparing.pools;
using UnityEngine;
using UnityEngine.UI;

namespace Preparing
{
    [Serializable]
    public class ShopItem
    {
        public string itemTitle;
    }

    public class ShopScrollList : MonoBehaviour
    {
//        public List<ShopItem> itemList;
        public Transform contentPanel;

        public ShopItemsPool itemsPool;

        public PickedShopScrollList otherList;

//        public ShopPickedItemsPool itemsPool;
//
//        //TODO: remove
//        public int money = 1000;
//
//        // Start is called before the first frame update
//        private void Start() { AddButtons(); }
//
//        private void AddButtons()
//        {
//            foreach (var item in itemList)
//            {
//                var itemButton = itemsPool.GetObject();
//                itemButton.transform.SetParent(contentPanel);
//
//                var shopItemButton = itemButton.GetComponent<ShopItemButton>();
//                shopItemButton.Setup(item, this);
//            }
//        }

        [Header("Other objects")] public GameObject cardPrefab;

        [Range(0, 500)] public int cardOffset;
        [Range(0f, 40f)] public float snapSpeed;

        public ScrollRect scrollRect;

        private GameObject[] _instCards = new GameObject[PreparingData.shopItems.Length];
        private Vector2[] _cardsPos = new Vector2[PreparingData.shopItems.Length];
        private Vector2[] _cardsScale = new Vector2[PreparingData.shopItems.Length];

        private RectTransform _contentRect;
        private Vector2 _contentVector;

        private int _selectedCardId;
        private bool _isScrolling;

        private void AddButtons()
        {
            for (var i = 0; i < PreparingData.shopItems.Length; i++)
            {
                var itemButton = itemsPool.GetObject();
                itemButton.transform.SetParent(contentPanel);

                var shopItemButton = itemButton.GetComponent<ShopItemButton>();
                shopItemButton.Setup(new ShopItem() {itemTitle = "Aue!!!"}, this);
            }
        }

        private void Start()
        {
            _contentRect = GetComponent<RectTransform>();

            for (var i = 0; i < PreparingData.shopItems.Length; i++)
            {
                var itemButton = itemsPool.GetObject();
                itemButton.transform.SetParent(contentPanel);

                var shopItemButton = itemButton.GetComponent<ShopItemButton>();
                shopItemButton.Setup(PreparingData.shopItems[i], this);

                _instCards[i] = itemButton;
                
                if (i == 0) continue;
                _instCards[i].transform.localPosition = new Vector2(_instCards[i - 1].transform.localPosition.x + cardPrefab.GetComponent<RectTransform>().sizeDelta.x +
                                                                    cardOffset, _instCards[i].transform.localPosition.y);
                _cardsPos[i] = -_instCards[i].transform.localPosition;
            }
            
            
            /*
            
            for (var i = 0; i < PreparingData.shopItems.Length; i++)
            {
                //Set data
                //var newText = cardPrefab.GetComponentInChildren<Text>();
                //newText.text = "Aue: " + i;

                //_instCards[i] = Instantiate(cardPrefab, transform, false);


                var itemButton = itemsPool.GetObject();
                itemButton.transform.SetParent(contentPanel);

                var shopItemButton = itemButton.GetComponent<ShopItemButton>();
                shopItemButton.Setup(new ShopItem() {itemTitle = "BBBB"}, this);

                _instCards[i] = itemButton;


//                _instCards[i] = (GameObject) GameObject.Instantiate(cardPrefab, contentPanel, true);

//                var shopItemButton = _instCards[i].GetComponent<ShopItemButton>();
//                var item = new ShopItem() {itemTitle = "Title 1"};
//                shopItemButton.Setup(item, this);
//
//                var shopItemButton = _instCards[i].GetComponent<ShopItemButton>();
//                if (shopItemButton != null)
//                {
//                    shopItemButton.Setup(new ShopItem {itemTitle = "Null title"}, this);
//                }


                // Set coordinates
                if (i == 0) continue;
                _instCards[i].transform.localPosition = new Vector2(_instCards[i - 1].transform.localPosition.x + cardPrefab.GetComponent<RectTransform>().sizeDelta.x +
                    cardOffset, _instCards[i].transform.localPosition.y);
                _cardsPos[i] = -_instCards[i].transform.localPosition;
                
                
            }
            
            
            
            */

        }

        private void FixedUpdate()
        {
            if (_contentRect.anchoredPosition.x >= _cardsPos[0].x && !_isScrolling ||
                _contentRect.anchoredPosition.x <= _cardsPos[_cardsPos.Length - 1].x && !_isScrolling)
            {
                scrollRect.inertia = false;
            }

            var nearestPosition = float.MaxValue;
            for (var i = 0; i < PreparingData.shopItems.Length; i++)
            {
                var distance = Mathf.Abs(_contentRect.anchoredPosition.x - _cardsPos[i].x);
                if (!(distance < nearestPosition)) continue;
                nearestPosition = distance;
                _selectedCardId = i;
            }

//            for (var i = 0; i < PreparingData.preparingCardsCount; i++)
//            {
//                var distance = Mathf.Abs(_contentRect.anchoredPosition.x - _cardsPos[i].x);
//                var scale = Mathf.Clamp(nearestPosition == distance ? 0.7f : 0.5f, 0.5f, 1f);
//
//                _cardsScale[i].x = Mathf.SmoothStep(_instCards[i].transform.localScale.x, scale + 0.15f,
//                    10 * Time.fixedDeltaTime);
//                _cardsScale[i].y = Mathf.SmoothStep(_instCards[i].transform.localScale.y, scale + 0.15f,
//                    10 * Time.fixedDeltaTime);
//                _instCards[i].transform.localScale = _cardsScale[i];
//            }

            var scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
            if (scrollVelocity < 400 && !_isScrolling)
            {
                scrollRect.inertia = false;
            }

            if (_isScrolling || scrollVelocity > 400) return;
            _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _cardsPos[_selectedCardId].x,
                snapSpeed * Time.fixedDeltaTime);
            _contentRect.anchoredPosition = _contentVector;
        }

        public void Scrolling(bool scroll)
        {
            _isScrolling = scroll;
            if (scroll)
            {
                scrollRect.inertia = true;
            }
        }
    }
}