using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict15 : Card
    {
        public CardFict15()
            : base(15, 50, CardType.GOLD, StuffClass.FICT, false,
                  "Если ваш уровень силы меньше, чем у противника, " +
                  "все ваши персонажи с других клеток присоединяются к бою")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = controller.GetAllCharacters();
            characters.RemoveAll(character => character.GetPlayer() != battle.GetPlayer());

            foreach (Character character in characters)
            {
                controller.MoveCharacter(character, battle.GetCell());
            }
        }

        public override void Choose(Chooser chooser) { }
    }
}