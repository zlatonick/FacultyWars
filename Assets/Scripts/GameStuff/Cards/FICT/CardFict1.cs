using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict1 : Card
    {
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

        public override void Choose(Chooser chooser)
        {
            chosenCharLose = chooser.ChooseCharacter("Выберите персонажа, который лишится силы");
            chosenCharIncrease = chooser.ChooseCharacter(
                "Выберите персонажа, к которому перейдет сила");
        }
    }
}