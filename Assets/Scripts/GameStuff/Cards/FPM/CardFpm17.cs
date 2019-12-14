using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm17 : Card
    {
        public CardFpm17()
            : base(17, 30, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Вы получаете +10 силе, за каждую серебряную карту у вас в руке")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());
            List<Card> cards = playerInfo.GetCardsInHand();

            int powerBonus = 0;

            foreach (Card card in cards)
            {
                if (card.GetCardType() == CardType.SILVER)
                {
                    powerBonus += 10;
                }
            }

            controller.ChangePowerSafe(battle.GetCharacter(), powerBonus);
        }

        
    }
}