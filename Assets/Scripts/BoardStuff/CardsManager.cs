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

        private Dictionary<int, GameObject> cardInstances;

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
            cardInstances = new Dictionary<int, GameObject>();
            lastCardId = 0;

            // Calculating some metrics
            var cardPrefabRect = iasaCards[0].GetComponent<RectTransform>();
            cardWidth = cardPrefabRect.sizeDelta.x * cardPrefabRect.localScale.x;

            RectTransform cardPanelRect = GetComponent<RectTransform>();
            cardPanelWidth = cardPanelRect.rect.xMax - cardPanelRect.rect.xMin;
            cardPanelWidth = cardPanelRect.rect.yMax - cardPanelRect.rect.yMin;

            cardsDistance = cardPanelWidth / 20;
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

            if (cardsQuan * (cardWidth + cardsDistance) > cardPanelWidth)
            {
                cardsDistance = cardPanelWidth / cardsQuan - cardWidth;
            }
            else
            {
                cardsDistance = cardPanelWidth / 20;
            }

            for (int i = 0; i < cardsQuan; i++)
            {
                cardInstances[i].transform.localPosition = new Vector2(
                    i * (cardWidth + cardsDistance), 0);
            }
        }

        public int AddCard(StuffClass stuffClass, CardType cardType, string text)
        {
            // Creating the card
            CardClickHandler cardSource = null;

            // No_battle texture = neutral
            if (cardType == CardType.NO_BATTLE)
            {
                cardType = CardType.NEUTRAL;
            }

            if (stuffClass == StuffClass.IASA) cardSource = iasaCards[(int)cardType];
            else if (stuffClass == StuffClass.FICT) cardSource = fictCards[(int)cardType];
            else if (stuffClass == StuffClass.FPM) cardSource = fpmCards[(int)cardType];

            GameObject newInst = Instantiate(cardSource.gameObject, transform, false);

            // Setting the parameters
            CardClickHandler card = newInst.GetComponent<CardClickHandler>();
            card.SetId(lastCardId);
            card.SetCardPlayedAction(cardPlayedAction);
            card.SetCanvas(canvas);
            card.SetCanDragPredicate(canPlayCard);

            var cardText = newInst.GetComponentInChildren<Text>();
            cardText.text = text;

            // Adding to the list
            cardInstances.Add(lastCardId, newInst);
            lastCardId++;

            // Fixing the positions
            int cardsQuan = cardInstances.Count;

            if (cardsQuan * (cardWidth + cardsDistance) > cardPanelWidth)
            {
                FixCardsPositions();
            }
            else
            {
                newInst.transform.localPosition = new Vector2(
                    (cardsQuan - 1) * (cardWidth + cardsDistance), 0);
            }

            return lastCardId - 1;
        }

        public void RemoveCard(int cardId)
        {
            GameObject card = cardInstances[cardId];

            cardInstances.Remove(cardId);

            Destroy(card);

            FixCardsPositions();
        }
    }
}
