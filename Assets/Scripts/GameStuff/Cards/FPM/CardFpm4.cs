using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm4 : Card
    {
        public CardFpm4()
            : base(4, 60, CardType.GOLD, StuffClass.FPM, false,
                  "+20 к силе вашему персонажу в бою за каждую открытую клетку на поле")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Cell> cells = new List<Cell>(controller.GetAllCells());
            cells.RemoveAll(cell => cell.GetState() == CellState.CLOSED);

            controller.ChangePowerSafe(battle.GetCharacter(), cells.Count * 20);
        }

        
    }
}