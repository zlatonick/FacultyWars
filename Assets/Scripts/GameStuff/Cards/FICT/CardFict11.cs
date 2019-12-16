using BoardStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict11 : Card
    {
        private Chooser chooser;

        private Action<Card> afterChoosingAction;

        private Character character1;

        private Character character2;

        public CardFict11()
            : base(11, 20, CardType.NO_BATTLE, StuffClass.FICT, true,
                  "Вы меняете местами двух ваших персонажей")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Cell сell1 = controller.GetCharacterCell(character1);
            Cell сell2 = controller.GetCharacterCell(character2);

            controller.MoveCharacter(character1, сell2);
            controller.MoveCharacter(character2, сell1);
        }

        public override void Choose(Chooser chooser, Action<Card> afterChoosingAction)
        {
            this.chooser = chooser;
            this.afterChoosingAction = afterChoosingAction;
            chooser.ChooseCharacter("Выберите первого персонажа", ChooseSecond);
        }

        public void ChooseSecond(Character character1)
        {
            this.character1 = character1;
            chooser.ChooseCharacter("Выберите второго персонажа", AfterBothWereChosen);
        }

        public void AfterBothWereChosen(Character character2)
        {
            this.character2 = character2;
            afterChoosingAction(this);
        }
    }
}