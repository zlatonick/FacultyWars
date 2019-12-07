using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict5 : Card
    {
        public CardFict5()
            : base(5, 40, CardType.NEUTRAL, StuffClass.FICT, false,
                  "Если вы выиграете бой, вы вернете в руку случайного вашего персонажа, погибшего в матче")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.SetAfterBattleAction(winner =>
            {
                if (winner != null && winner == battle.GetPlayer())
                {
                    PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());
                    List<Check> deadChars = playerInfo.GetChecksDead();

                    Random random = new Random();
                    int checkIndex = random.Next(0, deadChars.Count);

                    playerInfo.AddCheckToHand(deadChars[checkIndex]);
                }
            });
        }

        public override void Choose(Chooser chooser) { }
    }
}