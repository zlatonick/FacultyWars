using MetaInfo;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CardsManager : MonoBehaviour
    {
        public Canvas canvas;

        // 0 - gold, 1 - silver, 2 - neutral
        public CardClickHandler[] iasaCards;
        public CardClickHandler[] fictCards;
        public CardClickHandler[] fpmCards;

        private Dictionary<int, CardClickHandler> cardInstances;

        // Some metrical characteristics
        private int lastCardId;

        private float cardWidth;
        private float cardPanelWidth;
        private float cardPanelHeight;
        private float cardsDistance;

        // Card played action
        private Action<int> cardPlayedAction;
        private Func<int, bool> canPlayCard;

        void Start()
        {
            cardInstances = new Dictionary<int, CardClickHandler>();
            lastCardId = 0;

            // Calculating some metrics
            var cardPrefabRect = iasaCards[0].GetComponent<RectTransform>();
            cardWidth = cardPrefabRect.sizeDelta.x * cardPrefabRect.localScale.x;

            RectTransform cardPanelRect = GetComponent<RectTransform>();
            cardPanelWidth = cardPanelRect.rect.xMax - cardPanelRect.rect.xMin;
            cardPanelHeight = cardPanelRect.rect.yMax - cardPanelRect.rect.yMin;

            cardsDistance = cardPanelWidth / 40;
        }

        public void SetCardPlayedAction(Action<int> cardPlayedAction)
        {
            this.cardPlayedAction = cardPlayedAction;
        }

        public void SetCanPlayCardPredicate(Func<int, bool> canPlayCard)
        {
            this.canPlayCard = canPlayCard;
        }

        private void FixCardsPositions()
        {
            int cardsQuan = cardInstances.Count;

            float totalCardsWidth = cardsQuan * (cardWidth + cardsDistance) - cardsDistance;

            if (totalCardsWidth < cardPanelWidth)
            {
                float margin = (cardPanelWidth - totalCardsWidth) / 2;

                int cardIndex = 0;
                foreach (var card in cardInstances)
                {
                    float coordX = margin + cardIndex * (cardWidth + cardsDistance);
                    card.Value.transform.localPosition = new Vector2(coordX, 0);
                    card.Value.FixBigPrefabPosition();

                    cardIndex++;
                }
            }
            else
            {
                float cardsInnerDist = (cardPanelWidth - cardWidth) / (cardsQuan - 1);

                int cardIndex = 0;
                foreach (var card in cardInstances)
                {
                    float coordX = cardIndex * cardsInnerDist;
                    card.Value.transform.localPosition = new Vector2(coordX, 0);
                    card.Value.FixBigPrefabPosition();

                    cardIndex++;
                }
            }
        }

        public GameObject GetCardGameObject(StuffClass stuffClass, CardType cardType)
        {
            // Creating the card
            GameObject cardSource = null;

            // No_battle texture = neutral
            if (cardType == CardType.NO_BATTLE)
            {
                cardType = CardType.NEUTRAL;
            }

            if (stuffClass == StuffClass.IASA) cardSource = iasaCards[(int)cardType].gameObject;
            else if (stuffClass == StuffClass.FICT) cardSource = fictCards[(int)cardType].gameObject;
            else if (stuffClass == StuffClass.FPM) cardSource = fpmCards[(int)cardType].gameObject;

            return cardSource;
        }

        public int AddCard(StuffClass stuffClass, CardType cardType, string text)
        {
            GameObject cardSource = GetCardGameObject(stuffClass, cardType);
            GameObject newInst = Instantiate(cardSource, transform, false);

            // Setting the parameters
            CardClickHandler card = newInst.GetComponent<CardClickHandler>();
            card.SetId(lastCardId);
            card.SetCardPlayedAction(cardPlayedAction);
            card.SetCanvas(canvas);
            card.SetCanDragPredicate(canPlayCard);

            var cardText = newInst.GetComponentInChildren<Text>();
            cardText.text = text;

            card.InitCard();

            // Adding to the list
            cardInstances.Add(lastCardId, card);
            lastCardId++;

            // Fixing the positions
            FixCardsPositions();

            return lastCardId - 1;
        }

        public void RemoveCard(int cardId)
        {
            CardClickHandler card = cardInstances[cardId];

            cardInstances.Remove(cardId);

            Destroy(card.gameObject);

            FixCardsPositions();
        }
    }
}
