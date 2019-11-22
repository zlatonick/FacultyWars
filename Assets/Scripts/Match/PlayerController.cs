using System;
using System.Collections.Generic;
using GameStuff;
using MetaInfo;

namespace Match
{
    public class PlayerController : PlayerInfo
    {
        StuffClass stuffClass;

        Dictionary<int, Card> cards;

        List<Check> checks;

        List<Card> cardsPlayed;

        List<Check> checksDead;

        List<int> battlesHistory;

        CardsManager cardsManager;

        public PlayerController(StuffClass stuffClass, CardsManager cardsManager)
        {
            this.stuffClass = stuffClass;
            this.cardsManager = cardsManager;

            cards = new Dictionary<int, Card>();
            checks = new List<Check>();
            cardsPlayed = new List<Card>();
            checksDead = new List<Check>();
            battlesHistory = new List<int>();
        }

        public StuffClass GetStuffClass()
        {
            return stuffClass;
        }

        public void AddCardToHand(Card card)
        {
            int cardId = cardsManager.AddCard(
                card.GetStuffClass(), card.GetCardType(), card.GetText());

            cards.Add(cardId, card);
        }

        public void AddCheckToHand(Check check)
        {
            checks.Add(check);
        }

        public List<int> GetBattlesHistory()
        {
            return battlesHistory;
        }

        public List<Card> GetCardsInHand()
        {
            return new List<Card>(cards.Values);
        }

        public List<Card> GetCardsPlayed()
        {
            return cardsPlayed;
        }

        public List<Check> GetChecksDead()
        {
            return checksDead;
        }

        public List<Check> GetChecksInHand()
        {
            return checks;
        }

        public void RemoveCardFromHand(Card card)
        {
            throw new NotImplementedException();
        }

        public void RemoveCheckFromHand(Check check)
        {
            checks.Remove(check);
        }

        public void SetPlayingCardAction(Action<Card> action)
        {
            throw new NotImplementedException();
        }
    }
}
