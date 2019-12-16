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

        int GetChecksCount();

        List<Card> GetCardsPlayed();

        List<Check> GetChecksDead();

        List<int> GetBattlesHistory();

        void AddCheckToHand(Check check);

        void RemoveCheckFromHand(int checkLevel);

        void AddCardToHand(Card card);

        void SetActionsPermission(bool permission);

        void SetAllowedCharacters(bool allowed);

        void AddCheckToDead(Check check);

        void AddCardToPlayed(Card card);
    }
}
