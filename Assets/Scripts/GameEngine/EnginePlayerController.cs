using System;
using System.Collections.Generic;
using BoardStuff;
using GameStuff;
using MetaInfo;

namespace GameEngine
{
    public class EnginePlayerController : PlayerInfo
    {
        public void AddCardToHand(Card card)
        {

        }

        public void AddCardToPlayed(Card card)
        {

        }

        public void AddCheckToDead(Check check)
        {

        }

        public void AddCheckToHand(Check check)
        {

        }

        public List<int> GetBattlesHistory()
        {
            return null;
        }

        public List<Card> GetCardsInHand()
        {
            return null;
        }

        public List<Card> GetCardsPlayed()
        {
            return null;
        }

        public List<Check> GetChecksDead()
        {
            return null;
        }

        public Dictionary<int, int> GetChecksInHand()
        {
            return null;
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