using MetaInfo;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CardsDemonstrator : MonoBehaviour
    {
        public CardsManager cardsManager;
        
        private GameObject displayedCard;

        public void DemonstrateCard(StuffClass stuffClass, CardType cardType, string text)
        {
            GameObject cardObj = cardsManager.GetCardGameObject(stuffClass, cardType);
            displayedCard = Instantiate(cardObj, transform, false);

            // Removing the script component
            Destroy(displayedCard.GetComponent<CardClickHandler>());

            var cardText = displayedCard.GetComponentInChildren<Text>();
            cardText.text = text;
            
            RectTransform rect = displayedCard.GetComponent<RectTransform>();

            displayedCard.transform.localScale = new Vector2(rect.localScale.x * 3, rect.localScale.y * 3);

            float cardWidth = rect.sizeDelta.x * rect.localScale.x;
            displayedCard.transform.localPosition = new Vector2(-(cardWidth / 2), 0);

            displayedCard.transform.SetAsFirstSibling();      // Moving back

            StartCoroutine(RemoveCard());
        }

        private IEnumerator RemoveCard()
        {
            yield return new WaitForSeconds(2.5f);

            Destroy(displayedCard);
            displayedCard = null;
        }
    }
}