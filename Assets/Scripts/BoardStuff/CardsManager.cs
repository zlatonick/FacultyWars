using MetaInfo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CardsManager : MonoBehaviour
    {
        public GameObject cardPrefab;

        private List<GameObject> cardInstances;

        // Some metrical characteristics
        private int cardsQuan;

        private float cardWidth;
        private float cardPanelWidth;
        private float cardsDistance;

        // Start is called before the first frame update
        void Start()
        {
            cardInstances = new List<GameObject>();
            cardsQuan = 0;

            var cardPrefabRect = cardPrefab.GetComponent<RectTransform>();
            cardWidth = cardPrefabRect.sizeDelta.x * cardPrefabRect.localScale.x;

            RectTransform cardPanelRect = GetComponent<RectTransform>();
            cardPanelWidth = cardPanelRect.rect.xMax - cardPanelRect.rect.xMin;

            cardsDistance = cardPanelWidth / 20;
        }

        private void FixCardsPositions()
        {
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
                    i * (cardWidth + cardsDistance),
                    cardInstances[i].transform.localPosition.y);
            }
        }

        public int AddCard(StuffClass stuffClass, CardType cardType, string text)
        {
            // Creating the card
            GameObject newInst = Instantiate(cardPrefab, transform, false);

            var cardText = cardPrefab.GetComponentInChildren<Text>();
            cardText.text = text;

            // Adding to the list
            cardInstances.Add(newInst);
            cardsQuan++;

            // Fixing the positions
            if (cardsQuan * (cardWidth + cardsDistance) > cardPanelWidth)
            {
                FixCardsPositions();
            }
            else
            {
                newInst.transform.localPosition = new Vector2(
                    (cardsQuan - 1) * (cardWidth + cardsDistance),
                    newInst.transform.localPosition.y);
            }

            return cardsQuan - 1;
        }

        public void RemoveCard(int index)
        {
            GameObject card = cardInstances[index];

            cardInstances.RemoveAt(index);
            cardsQuan++;

            Destroy(card);

            FixCardsPositions();
        }
    }
}
