using BoardStuff;
using MetaInfo;
using System;

namespace GameStuff
{
    public class CardFict9 : Card
    {
        private Action<Card> afterChoosingAction;

        private Character characterToMove;

        public CardFict9()
            : base(9, 60, CardType.SILVER, StuffClass.FICT, true,
                  "В бой перемещается ваш выбранный персонаж с соседней клетки. " +
                  "Силы ваших персонажей суммируются")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.MoveCharacter(characterToMove, battle.GetCell());
        }

        public override void Choose(Chooser chooser, Action<Card> afterChoosingAction)
        {
            this.afterChoosingAction = afterChoosingAction;
            chooser.ChooseCharacter("Выберите персонажа для перемещения", SetCharacterToMove);
        }

        public void SetCharacterToMove(Character characterToMove)
        {
            this.characterToMove = characterToMove;
            afterChoosingAction(this);
        }
    }
}