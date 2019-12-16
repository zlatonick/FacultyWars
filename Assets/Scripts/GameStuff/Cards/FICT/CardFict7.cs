using BoardStuff;
using MetaInfo;
using System;

namespace GameStuff
{
    public class CardFict7 : Card
    {
        private Chooser chooser;

        private Action<Card> afterChoosingAction;

        private Cell chosenCell;

        public CardFict7()
            : base(7, 30, CardType.NO_BATTLE, StuffClass.FICT, true,
                  "Блокирует клетку от персонажей противника на 2 хода")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            chosenCell.SetBlock(true);

            controller.SetAfterNTurnsAction(2, () =>
            {
                chosenCell.SetBlock(false);
            });
        }

        public override void Choose(Chooser chooser, Action<Card> afterChoosingAction)
        {
            this.chooser = chooser;
            this.afterChoosingAction = afterChoosingAction;
            chooser.ChooseCell("Выберите клетку", AfterCellWasChosen);
        }

        public void AfterCellWasChosen(Cell chosenCell)
        {
            this.chosenCell = chosenCell;
            afterChoosingAction(this);
        }
    }
}