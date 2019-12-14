using System;
using System.Collections.Generic;
using BoardStuff;
using GameStuff;
using MetaInfo;

namespace GameEngine
{
    public class EnginePlayerController : PlayerInfo
    {
        private StuffClass stuffClass;

        private List<Card> cards;

        private Dictionary<int, Check> checkLevels;      // int is check level

        private Dictionary<int, int> checks;          // Each check type count

        private List<Card> cardsPlayed;

        private List<Check> checksDead;

        private List<int> battlesHistory;

        public EnginePlayerController(StuffClass stuffClass,
            Dictionary<int, Check> checkLevels)
        {
            this.stuffClass = stuffClass;
            this.checkLevels = checkLevels;

            cards = new List<Card>();
            cardsPlayed = new List<Card>();
            checksDead = new List<Check>();
            battlesHistory = new List<int>();

            checks = new Dictionary<int, int>();
            foreach (var pr in checkLevels)
            {
                checks.Add(pr.Key, 0);
            }
        }

        public void AddCardToHand(Card card)
        {
            cards.Add(card);
        }

        public void RemoveCardFromHand(Card card)
        {
            cards.Remove(card);
        }

        public void AddCardToPlayed(Card card)
        {
            cardsPlayed.Add(card);
        }

        public void AddCheckToDead(Check check)
        {
            checksDead.Add(check);
        }

        public void AddCheckToHand(Check check)
        {
            checks[check.GetLevel()]++;
        }

        public void RemoveCheckFromHand(int checkLevel)
        {
            checks[checkLevel]--;
        }

        public List<int> GetBattlesHistory()
        {
            return battlesHistory;
        }

        public List<Card> GetCardsInHand()
        {
            return cards;
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

        public void SetActionAfterCardIsPlayed(Action<Card> action)
        {

        }

        public void SetActionsPermission(bool permission)
        {

        }

        public void SetAllowedCharacters(bool allowed)
        {

        }

        public void SetCanPlayCardPredicate(Func<CardType> canPlayCardNow)
        {

        }

        public void SetCardPlayedAction(Action<Card> cardPlayedAction)
        {

        }

        public void SetCheckPlacedAction(Action<Check, Cell> checkPlacedAction)
        {

        }
    }
}