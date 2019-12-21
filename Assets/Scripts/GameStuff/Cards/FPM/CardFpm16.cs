using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm16 : Card
    {
        public CardFpm16()
            : base(16, 50, CardType.SILVER, StuffClass.FPM, false,
                  "Вы повторяете случайную золотую карту, разыгранную противником в этом матче. ")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo opponentInfo = controller.GetPlayerInfo(battle.GetEnemyPlayer());
            List<Card> cards = new List<Card>(opponentInfo.GetCardsPlayed());

            cards.RemoveAll(card => card.GetCardType() != CardType.GOLD);

            if (cards.Count == 0)
            {
                cards = new List<Card>(opponentInfo.GetCardsPlayed());
                cards.RemoveAll(card => card.GetCardType() != CardType.SILVER);
            }

            if (cards.Count != 0)
            {
                Random random = new Random();
                cards[random.Next(cards.Count)].Act(battle, controller);
            }
        }

        
    }
}