using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm14 : Card
    {
        CardFactory cardFactory;

        public CardFpm14()
            : base(14, 90, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Вы кладете в руку случайную серебряную карту класса вашего противника " +
                  "за каждого персонажа противника на поле")
        {
            cardFactory = new CardFactoryImpl();
        }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = controller.GetAllCharacters();
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            foreach (Character character in characters)
            {
                if (character.GetPlayer() == battle.GetEnemyPlayer())
                {
                    playerInfo.AddCardToHand(cardFactory.GetRandomCard(
                        battle.GetEnemyPlayer().stuffClass, CardType.SILVER));
                }
            }
        }

        
    }
}