using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm12 : Card
    {
        CheckFactory checkFactory;

        public CardFpm12()
            : base(12, 80, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Если на поле есть 3 открытых клетки, вы получаете персонажа с силой 50")
        {
            checkFactory = new CheckFactoryImpl();
        }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Cell> cells = new List<Cell>(controller.GetAllCells());
            cells.RemoveAll(cell => cell.GetState() != CellState.CLOSED);

            if (cells.Count >= 3)
            {
                PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());
                playerInfo.AddCheckToHand(checkFactory.GetCheck(StuffClass.FPM, 1));
            }
        }

        
    }
}