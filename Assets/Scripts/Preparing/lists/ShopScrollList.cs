using System;
using System.Collections.Generic;
using GameStuff;
using MetaInfo;
using Preparing.list_items;
using Preparing.pools;
using UnityEngine;
using UnityEngine.UI;

namespace Preparing.lists
{
    
    public class ShopScrollList : MonoBehaviour
    {
        public Transform contentPanel;

        public ShopItemsPool itemsPool;

        public PickedShopScrollList otherList;

        public Sprite spriteGoldCardFict;
        public Sprite spriteSilverCardFict;
        public Sprite spriteNeutralCardFict;

        public Sprite spriteGoldCardIpsa;
        public Sprite spriteSilverCardIpsa;
        public Sprite spriteNeutralCardIpsa;
        
        public Sprite spriteGoldCardFpm;
        public Sprite spriteSilverCardFpm;
        public Sprite spriteNeutralCardFpm;
        
        [Header("Other objects")] public GameObject cardPrefab;

        [Range(0, 500)] public int cardOffset;
        [Range(0f, 40f)] public float snapSpeed;

        public ScrollRect scrollRect;

        private GameObject[] _instCards = new GameObject[PreparingData.cardCount];
        private Vector2[] _cardsPos = new Vector2[PreparingData.cardCount];
        private Vector2[] _cardsScale = new Vector2[PreparingData.cardCount];

        private RectTransform _contentRect;
        private Vector2 _contentVector;

        private int _selectedCardId;
        private bool _isScrolling;

        private void Start()
        {
            _contentRect = GetComponent<RectTransform>();
            var cardFactory = new CardFactoryImpl();
            
            for (var i = 0; i < PreparingData.cardCount; i++)
            {
                var card = cardFactory.GetCard(StuffPack.stuffClass, i);
                var itemButton = itemsPool.GetObject();
                itemButton.transform.SetParent(contentPanel);
                
                List<Sprite> sprites = new List<Sprite>();
                switch (StuffPack.stuffClass)
                {
                    case StuffClass.FICT:
                        sprites = new List<Sprite>() { spriteGoldCardFict, spriteSilverCardFict, spriteNeutralCardFict };
                        break;
                    case StuffClass.IASA:
                        sprites = new List<Sprite>() { spriteGoldCardIpsa, spriteSilverCardIpsa, spriteNeutralCardIpsa };
                        break;
                    case StuffClass.FPM:
                        sprites = new List<Sprite>() { spriteGoldCardFpm, spriteSilverCardFpm, spriteNeutralCardFpm };
                        break;
                }
                
                var shopItemButton = itemButton.GetComponent<ShopItemButton>();
                shopItemButton.Setup(card, this, sprites);

                _instCards[i] = itemButton;
                
                if (i == 0) continue;
                _instCards[i].transform.localPosition = new Vector2(_instCards[i - 1].transform.localPosition.x + cardPrefab.GetComponent<RectTransform>().sizeDelta.x +
                                                                    cardOffset, _instCards[i].transform.localPosition.y);
                _cardsPos[i] = -_instCards[i].transform.localPosition;
            }
        }

        private void FixedUpdate()
        {
            if (_contentRect.anchoredPosition.x >= _cardsPos[0].x && !_isScrolling ||
                _contentRect.anchoredPosition.x <= _cardsPos[_cardsPos.Length - 1].x && !_isScrolling)
            {
                scrollRect.inertia = false;
            }

            var nearestPosition = float.MaxValue;
            for (var i = 0; i < PreparingData.cardCount; i++)
            {
                var distance = Mathf.Abs(_contentRect.anchoredPosition.x - _cardsPos[i].x);
                if (!(distance < nearestPosition)) continue;
                nearestPosition = distance;
                _selectedCardId = i;
            }

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