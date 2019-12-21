using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict16 : Card
    {
        public CardFict16()
            : base(16, 90, CardType.NEUTRAL, StuffClass.FICT, false,
                  "Каждый раз, когда вы разыгрывает серебряную карту в данном бою, " +
                  "вы воскрешаете случайного своего персонажа")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            controller.SetActionAfterCardIsPlayed(battle.GetPlayer(), card =>
            {
                if (card.GetCardType() == CardType.SILVER)
                {
                    List<Check> deadChars = playerInfo.GetChecksDead();

                    Random random = new Random();
                    int checkIndex = random.Next(0, deadChars.Count);

                    playerInfo.AddCheckToHand(deadChars[checkIndex]);
                }
            });
        }

        
    }
}