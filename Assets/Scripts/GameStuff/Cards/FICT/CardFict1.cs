using BoardStuff;
using MetaInfo;
using System;

namespace GameStuff
{
    public class CardFict1 : Card
    {
        private Chooser chooser;

        private Action<Card> afterChoosingAction;

        private Character chosenCharLose;

        private Character chosenCharIncrease;

        public CardFict1()
            : base(1, 30, CardType.SILVER, StuffClass.FICT, true,
                  "Вся сила вашего выбранного персонажа на поле переходит " +
                  "к другому вашему персонажу в бою. Персонаж, лишившийся силы, погибает")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            int power = chosenCharLose.GetPower();

            controller.ChangePowerSafe(chosenCharLose, -power);
            controller.ChangePowerSafe(chosenCharIncrease, power);
        }

        public override void Choose(Chooser chooser, Action<Card> afterChoosingAction)
        {
            this.chooser = chooser;
            this.afterChoosingAction = afterChoosingAction;
            chooser.ChooseCharacter("Выберите персонажа, который лишится силы", ChooseSecond);
        }

        private void ChooseSecond(Character chosenCharLose)
        {
            this.chosenCharLose = chosenCharLose;
            chooser.ChooseCharacter("Выберите персонажа, к которому перейдет сила", AfterBothWereChosen);

        }

        public void AfterBothWereChosen(Character chosenCharIncrease)
        {
            this.chosenCharIncrease = chosenCharIncrease;
            afterChoosingAction(this);
        }
    }
}