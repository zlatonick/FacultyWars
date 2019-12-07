using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict11 : Card
    {
        Character character1;
        Character character2;

        public CardFict11()
            : base(11, 20, CardType.NO_BATTLE, StuffClass.FICT, true,
                  "Вы меняете местами двух ваших персонажей")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Cell сell1 = controller.GetCharacterCell(character1);
            Cell сell2 = controller.GetCharacterCell(character2);

            controller.MoveCharacter(character1, сell1);
            controller.MoveCharacter(character2, сell2);
        }

        public override void Choose(Chooser chooser) 
        {
            character1 = chooser.ChooseCharacter("Выберите первого персонажа");
            character2 = chooser.ChooseCharacter("Выберите второго персонажа");
        }
    }
}