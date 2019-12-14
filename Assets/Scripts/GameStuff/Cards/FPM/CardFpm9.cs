using BoardStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm9 : Card
    {
        public CardFpm9()
            : base(9, 50, CardType.NO_BATTLE, StuffClass.FPM, false,
                  "Вы возвращаете в руку 1 случайную использованную в этом бою карту " +
                  "(кроме этой) за каждый выигранный в этом матче бой")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            List<int> battles = new List<int>(playerInfo.GetBattlesHistory());
            battles.RemoveAll(res => res != 1);

            List<Card> usedCards = new List<Card>(playerInfo.GetCardsPlayed());
            usedCards.RemoveAll(card => card.GetId() == 9);

            Random random = new Random(); 

            for (int i = 0; i < battles.Count; i++)
            {
                playerInfo.AddCardToHand(usedCards[random.Next(usedCards.Count)]);
            }
        }

        
    }
}