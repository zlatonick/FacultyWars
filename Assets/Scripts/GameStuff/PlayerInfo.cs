using BoardStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public interface PlayerInfo
    {
        List<Card> GetCardsInHand();

        Dictionary<int, int> GetChecksInHand();

        List<Card> GetCardsPlayed();

        List<Check> GetChecksDead();

        List<int> GetBattlesHistory();

        void AddCheckToHand(Check check);

        void AddCardToHand(Card card);

        void SetCheckPlacedAction(Action<Check, Cell> checkPlacedAction);

        void SetCardPlayedAction(Action<Card> cardPlayedAction);

        void SetCanPlayCardPredicate(Func<CardType> canPlayCardNow);

        void SetActionAfterCardIsPlayed(Action<Card> action);

        void SetActionsPermission(bool permission);

        void SetAllowedCharacters(bool allowed);
    }
}
