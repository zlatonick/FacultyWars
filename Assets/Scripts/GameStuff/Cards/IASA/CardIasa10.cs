using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardIasa10 : Card
    {
        public CardIasa10()
            : base(10, 60, CardType.SILVER, StuffClass.IASA, false,
                  "Вы получаете +20 к силе за каждый выигранный в матче бой")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<int> battles = new List<int>(
                controller.GetPlayerInfo(battle.GetPlayer()).GetBattlesHistory());
            battles.RemoveAll(res => res != 1);

            controller.ChangePowerSafe(battle.GetCharacter(), battles.Count * 20);
        }

        
    }
}
