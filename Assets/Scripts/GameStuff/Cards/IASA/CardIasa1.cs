using MetaInfo;

namespace GameStuff
{
    public class CardIasa1 : Card
    {
        public CardIasa1()
            : base(1, 60, CardType.SILVER, StuffClass.IASA, false,
                  "-20 от силы противника в бою")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.ChangePowerSafe(battle.GetEnemyCharacter(), -20);
        }

        public override void Choose(Chooser chooser) { }
    }
}
