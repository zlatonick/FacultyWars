using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict2 : Card
    {
        public CardFict2()
            : base(2, 60, CardType.GOLD, StuffClass.FICT, false,
                  "+20 к силе вашему персонажу в бою за каждого вашего персонажа на поле")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = controller.GetAllCharacters();
            characters.RemoveAll(character => character.GetPlayer() != battle.GetPlayer());

            controller.ChangePowerSafe(battle.GetCharacter(), characters.Count * 20);
        }

        public override void Choose(Chooser chooser) { }
    }
}