using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardIasa9 : Card
    {
        public CardIasa9()
            : base(0, 60, CardType.SILVER, StuffClass.IASA, false,
                  "Вы получаете +20 к силе за каждый выигранный в матче бой")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<int> battles = controller.GetPlayerInfo(battle.GetPlayer()).GetBattlesHistory();
            battles.RemoveAll(res => res != 1);

            controller.ChangePowerSafe(battle.GetCharacter(), battles.Count * 20);
        }

        public override void Choose(Chooser chooser) { }
    }
}
