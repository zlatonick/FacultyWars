using MetaInfo;
using System;

namespace GameStuff
{
    public class CardFict12 : Card
    {
        private Check chosenCheck;

        private Action<Card> afterChoosingAction;

        public CardFict12()
            : base(12, 50, CardType.SILVER, StuffClass.FICT, true,
                  "Вы добавляете в текущий бой еще одного персонажа из руки")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.PlaceCheck(chosenCheck, battle.GetCell());
        }

        public override void Choose(Chooser chooser, Action<Card> afterChoosingAction)
        {
            this.afterChoosingAction = afterChoosingAction;
            chooser.ChooseCheck("Выберите персонажа из руки", AfterCheckWasChosen);
        }

        public void AfterCheckWasChosen(Check check)
        {
            chosenCheck = check;
            afterChoosingAction(this);
        }
    }
}