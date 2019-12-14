using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm18 : Card
    {
        public CardFpm18()
            : base(18, 60, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Вы кладете в руку нейтральную карту «+10 к силе» за каждую открытую клетку на поле")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Cell> cells = controller.GetAllCells();
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            foreach (Cell cell in cells)
            {
                if (cell.GetState() != CellState.CLOSED)
                {
                    playerInfo.AddCardToHand(new CardAll0(StuffClass.FPM));
                }
            }
        }

        
    }
}