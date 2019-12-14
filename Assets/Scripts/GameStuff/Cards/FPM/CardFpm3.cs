using BoardStuff;
using MetaInfo;
using System;

namespace GameStuff
{
    public class CardFpm3 : Card
    {
        private Cell chosenCell;

        private Action<Card> afterChoosingAction;

        public CardFpm3()
            : base(3, 30, CardType.NEUTRAL, StuffClass.FPM, true,
                  "Текущая клетка поля меняется местами с одной из соседних " +
                  "(по вашему выбору) и остается открытой. Новая клетка открывается")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.OpenCell(chosenCell, CellState.OPENED);
            CellEffect newEffect = chosenCell.GetEffect();

            Cell currCell = battle.GetCell();

            controller.ChangeCellEffect(chosenCell, currCell.GetEffect());
            controller.ChangeCellEffect(currCell, newEffect);
        }

        public override void Choose(Chooser chooser, Action<Card> afterChoosingAction)
        {
            this.afterChoosingAction = afterChoosingAction;
            chooser.ChooseCell("Выберите клетку", SetChosenCell);
        }

        public void SetChosenCell(Cell cell)
        {
            chosenCell = cell;
            afterChoosingAction(this);
        }
    }
}