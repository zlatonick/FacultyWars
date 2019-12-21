using MetaInfo;

namespace GameStuff
{
    public class CardFpm11 : Card
    {
        public CardFpm11()
            : base(11, 70, CardType.SILVER, StuffClass.FPM, false,
                  "Ваш противник больше не может разыгрывать карты в этом бою")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.ChangeBattleStatus(battle.GetEnemyPlayer(), Match.BattleStatus.TWO_CARDS_PLAYED);
        }

        
    }
}