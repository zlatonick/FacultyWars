using BoardStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm15 : Card
    {
        public CardFpm15()
            : base(15, 40, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Каждый раз, когда вы получаете бонус к силе в данном бою, " +
                  "случайная клетка поля открывается")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            battle.GetCharacter().SetChangePowerAction(changeBy =>
            {
                if (changeBy > 0)
                {
                    List<Cell> cells = new List<Cell>(controller.GetAllCells());
                    cells.RemoveAll(cell => cell.GetState() != CellState.CLOSED);

                    if (cells.Count > 0)
                    {
                        Random random = new Random();
                        controller.OpenCell(cells[random.Next(cells.Count)], CellState.OPENED);
                    }
                }
            });
        }

        
    }
}