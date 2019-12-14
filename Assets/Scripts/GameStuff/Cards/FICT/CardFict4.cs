using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict4 : Card
    {
        public CardFict4()
            : base(4, 40, CardType.SILVER, StuffClass.FICT, false,
                  "Если ваш персонаж стал на клетку первым, он получает +30 к силе")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = controller.GetCharactersOnCell(battle.GetCell());
            Character character = battle.GetCharacter();

            if (characters.IndexOf(character) < characters.IndexOf(battle.GetEnemyCharacter()))
            {
                controller.ChangePowerSafe(character, 30);
            }
        }

        
    }
}