using System;
using System.Collections.Generic;
using GameStuff;
using MetaInfo;
using UnityEngine;

namespace BoardStuff
{
    public class PlayerController : PlayerInfo
    {
        private StuffClass stuffClass;

        private Dictionary<int, Card> cards;

        private Dictionary<int, Check> checkLevels;      // int is check level

        private Dictionary<int, int> checks;          // Each check type count

        private List<Card> cardsPlayed;

        private List<Check> checksDead;

        private List<int> battlesHistory;

        private bool actionsAreAllowed;

        private Action<Check, Cell> checkPlacedAction;
        private Func<CardType> canPlayCardNow;
        private Action<Check> checkClickedAction;
        private Action<Card> cardPlayedAction;

        private CardsManager cardsManager;

        private CheckManager checkManager;

        public PlayerController(StuffClass stuffClass, CardsManager cardsManager,
            CheckManager checkManager, Dictionary<int, Check> checkLevels)
        {
            this.stuffClass = stuffClass;
            this.cardsManager = cardsManager;
            this.checkManager = checkManager;
            this.checkLevels = checkLevels;

            checkManager.SetDragFinishedAction(OnCheckPlaced);
            checkManager.SetCheckClickedAction(OnCheckClicked);
            cardsManager.SetCardPlayedAction(OnCardPlayed);
            cardsManager.SetCanPlayCardPredicate(CanPlayCard);

            cards = new Dictionary<int, Card>();
            cardsPlayed = new List<Card>();
            checksDead = new List<Check>();
            battlesHistory = new List<int>();

            checks = new Dictionary<int, int>();
            Dictionary<int, int> checkPowers = new Dictionary<int, int>();

            foreach (var pr in checkLevels)
            {
                checks.Add(pr.Key, 0);
                checkPowers.Add(pr.Key, pr.Value.GetPower());
            }

            checkManager.SetStuffClass(stuffClass, checkPowers);

            checkManager.SetCanDrag(false);
            actionsAreAllowed = false;
        }

        public void SetCheckPlacedAction(Action<Check, Cell> checkPlacedAction)
        {
            this.checkPlacedAction = checkPlacedAction;
        }

        public void SetCheckClickedAction(Action<Check> checkClickedAction)
        {
            this.checkClickedAction = checkClickedAction;
        }

        public void SetCardPlayedAction(Action<Card> cardPlayedAction)
        {
            this.cardPlayedAction = cardPlayedAction;
        }

        public void SetCanPlayCardPredicate(Func<CardType> canPlayCardNow)
        {
            this.canPlayCardNow = canPlayCardNow;
        }

        private void OnCheckPlaced(Cell cell, int checkLevel)
        {
            checks[checkLevel]--;
            checkPlacedAction(checkLevels[checkLevel], cell);
        }

        private void OnCheckClicked(int checkLevel)
        {
            checkClickedAction(checkLevels[checkLevel]);
        }

        private void OnCardPlayed(int cardId)
        {
            cardsManager.RemoveCard(cardId);

            Card card = cards[cardId];
            
            cards.Remove(cardId);
            cardsPlayed.Add(card);

            cardPlayedAction(card);
        }

        public bool CanPlayCard(int cardId)
        {
            if (actionsAreAllowed)
            {
                CardType maxCardType = canPlayCardNow();
                Card card = cards[cardId];

                return (int)card.GetCardType() >= (int)maxCardType;
            }
            else
            {
                return false;
            }
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

        public void SetActionsPermission(bool permission)
        {
            actionsAreAllowed = permission;
            checkManager.SetCanDrag(permission);
        }

        public void SetAllowedCharacters(bool allowed)
        {
            checkManager.SetCanDrag(allowed);
        }

        public void AddCheckToDead(Check check)
        {
            checksDead.Add(check);
        }

        public void AddCardToPlayed(Card card)
        {
            cardsPlayed.Add(card);
        }
    }
}
