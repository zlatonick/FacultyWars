using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFpm10 : Card
    {
        public CardFpm10()
            : base(10, 50, CardType.SILVER, StuffClass.FPM, false,
                  "Вы повторяете последнюю карту, сыгранную противником")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Card> opponentCards = controller.GetPlayerInfo(battle.GetEnemyPlayer()).GetCardsPlayed();
            opponentCards[opponentCards.Count - 1].Act(battle, controller);
        }

        
    }
}