using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public interface PlayerInfo
    {
        StuffClass GetStuffClass();

        List<Card> GetCardsInHand();

        List<Check> GetChecksInHand();

        List<Card> GetCardsPlayed();

        List<Check> GetChecksDead();

        List<int> GetBattlesHistory();

        void AddCheckToHand(Check check);

        void AddCardToHand(Card card);

        void RemoveCheckFromHand(Check check);

        void RemoveCardFromHand(Card card);

        void SetPlayingCardAction(Action<Card> action);
    }
}
