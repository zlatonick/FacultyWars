using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict0 : Card
    {
        public CardFict0()
            : base(0, 50, CardType.SILVER, StuffClass.FICT, false,
                  "+20 к силе всем вашим персонажам на поле")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = controller.GetAllCharacters();

            foreach (Character character in characters)
            {
                if (character.GetPlayer() == battle.GetPlayer())
                {
                    controller.ChangePowerSafe(character, 20);
                }
            }
        }
    }
}