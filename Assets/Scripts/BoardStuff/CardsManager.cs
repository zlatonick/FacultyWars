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

        public GameObject cardLightning;

        private Dictionary<int, CardClickHandler> cardInstances;

        private Dictionary<int, GameObject> cardLightnings;

        private CardClickHandler cardDragging;

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
            cardLightnings = new Dictionary<int, GameObject>();
            lastCardId = 0;

            // Calculating some metrics
            var cardPrefabRect = iasaCards[0].GetComponent<RectTransform>();
            cardWidth = cardPrefabRect.sizeDelta.x * cardPrefabRect.localScale.x;

            RectTransform cardPanelRect = GetComponent<RectTransform>();
            cardPanelWidth = cardPanelRect.rect.xMax - cardPanelRect.rect.xMin;
            cardPanelHeight = cardPanelRect.rect.yMax - cardPanelRect.rect.yMin;

            cardsDistance = cardPanelWidth / 40;
        }

        public void HighlightCards(List<int> availableCards)
        {
            foreach (int cardId in availableCards)
            {
                GameObject lightning = Instantiate(cardLightning, transform, false);

                CardClickHandler card = cardInstances[cardId];
                Vector2 cardPos = card.transform.localPosition;
                float cardsScaleX = card.transform.localScale.x;

                lightning.transform.localPosition = new Vector2(
                    cardPos.x - 50f / 2 * cardsScaleX, cardPos.y);

                lightning.transform.SetSiblingIndex(card.transform.GetSiblingIndex());

                cardLightnings.Add(cardId, lightning);
            }
        }

        public void UnhighlightCards()
        {
            foreach (var pr in cardLightnings)
            {
                Destroy(pr.Value);
            }
            cardLightnings.Clear();
        }

        public void SetCardPlayedAction(Action<int> cardPlayedAction)
        {
            this.cardPlayedAction = cardPlayedAction;
        }

        public void SetCanPlayCardPredicate(Func<int, bool> canPlayCard)
        {
            this.canPlayCard = canPlayCard;
        }

        private void DragStartedAction(int cardId)
        {
            HideCard(cardId);
            UnhighlightCards();
        }

        private void DragFinishedAction(int cardId)
        {
            Destroy(cardDragging.gameObject);
            cardDragging = null;

            cardPlayedAction(cardId);
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
            card.SetDragStartedAction(DragStartedAction);
            card.SetCardPlayedAction(DragFinishedAction);
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

        private void HideCard(int cardId)
        {
            cardDragging = cardInstances[cardId];
            cardInstances.Remove(cardId);

            FixCardsPositions();
        }
    }
}
