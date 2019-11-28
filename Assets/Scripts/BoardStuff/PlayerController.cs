using System;
using System.Collections.Generic;
using GameStuff;
using MetaInfo;
using UnityEngine;

namespace BoardStuff
{
    public class PlayerController : PlayerInfo
    {
        Dictionary<int, Card> cards;

        Dictionary<int, Check> checkLevels;      // int is check level

        Dictionary<int, int> checks;          // Each check type count

        List<Card> cardsPlayed;

        List<Check> checksDead;

        List<int> battlesHistory;

        Action<Check, Cell> checkPlacedAction;

        CardsManager cardsManager;

        CheckManager checkManager;

        public PlayerController(CardsManager cardsManager, CheckManager checkManager,
            Dictionary<int, Check> checkLevels)
        {
            this.cardsManager = cardsManager;
            this.checkManager = checkManager;
            this.checkLevels = checkLevels;

            checkManager.SetDragFinishedAction(OnCheckPlaced);

            cards = new Dictionary<int, Card>();
            cardsPlayed = new List<Card>();
            checksDead = new List<Check>();
            battlesHistory = new List<int>();

            checks = new Dictionary<int, int>();
            foreach (var pr in checkLevels)
            {
                checks.Add(pr.Key, 0);
            }
        }

        public void SetCheckPlacedAction(Action<Check, Cell> checkPlacedAction)
        {
            this.checkPlacedAction = checkPlacedAction;
        }

        private void OnCheckPlaced(Cell cell, int checkLevel)
        {
            checks[checkLevel]--;
            checkPlacedAction(checkLevels[checkLevel], cell);
        }

        public void AddCardToHand(Card card)
        {
            int cardId = cardsManager.AddCard(
                card.GetStuffClass(), card.GetCardType(), card.GetText());

            cards.Add(cardId, card);
        }

        public void AddCheckToHand(Check check)
        {
            checkManager.AddCheck(check.GetLevel());
            checks[check.GetLevel()]++;
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

        public Dictionary<int, int> GetChecksInHand()
        {
            return checks;
        }

        public void SetPlayingCardAction(Action<Card> action)
        {
            throw new NotImplementedException();
        }

        public void SetActionsPermission(bool permission)
        {
            // TODO
        }

        public void SetAllowedCardTypes(CardType cardType)
        {
            // TODO
        }

        public void SetAllowedCharacters(bool allowed)
        {
            checkManager.SetCanDrag(allowed);
        }
    }
}
