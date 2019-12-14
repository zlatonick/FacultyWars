using MetaInfo;

namespace GameStuff
{
    public class CardIasa7 : Card
    {
        public CardIasa7()
            : base(7, 150, CardType.GOLD, StuffClass.IASA, false,
                  "Безусловная победа в бою")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.FinishBattle(battle.GetPlayer());
        }

        
    }
}
