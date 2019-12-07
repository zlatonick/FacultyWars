using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFict9 : Card
    {
        Character characterToMove;

        public CardFict9()
            : base(9, 60, CardType.SILVER, StuffClass.FICT, true,
                  "В бой перемещается ваш выбранный персонаж с соседней клетки. " +
                  "Силы ваших персонажей суммируются. В случае проигрыша вы теряете обоих персонажей")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.MoveCharacter(characterToMove, battle.GetCell());
        }

        public override void Choose(Chooser chooser)
        {
            characterToMove = chooser.ChooseCharacter("Выберите персонажа для перемещения");
        }
    }
}