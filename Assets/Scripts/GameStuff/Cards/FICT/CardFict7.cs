using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict7 : Card
    {
        public CardFict7()
            : base(7, 30, CardType.NO_BATTLE, StuffClass.FICT, false,
                  "В течение 2 своих ходов противник не может атаковать вашего выбранного персонажа")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Cell cell = battle.GetCell();
            cell.SetBlock(true);

            controller.SetAfterNTurnsAction(2, () =>
            {
                cell.SetBlock(false);
            });
        }

        
    }
}