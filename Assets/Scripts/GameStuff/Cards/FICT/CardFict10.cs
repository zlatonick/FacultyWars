using BoardStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict10 : Card
    {
        public CardFict10()
            : base(10, 50, CardType.SILVER, StuffClass.FICT, false,
                  "Если у вас на поле есть 3 персонажа, вы получаете +20 к силе " +
                  "и воскрешаете случайного вашего персонажа")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = new List<Character>(controller.GetAllCharacters());
            characters.RemoveAll(character => character.GetPlayer() != battle.GetPlayer());

            if (characters.Count >= 3)
            {
                controller.ChangePowerSafe(battle.GetCharacter(), 20);

                PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());
                List<Check> deadChars = playerInfo.GetChecksDead();

                Random random = new Random();
                int checkIndex = random.Next(0, deadChars.Count);

                playerInfo.AddCheckToHand(deadChars[checkIndex]);
            }
        }
    }
}