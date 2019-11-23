using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public interface PlayerInfo
    {
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

        void SetActionsPermission(bool permission);

        void SetAllowedCardTypes(CardType cardType);

        void SetAllowedCharacters(bool allowed);
    }
}
