using MetaInfo;

namespace GameStuff
{
    public class CardIasa6 : Card
    {
        public CardIasa6()
            : base(6, 100, CardType.GOLD, StuffClass.IASA, false,
                  "Если ваш уровень силы меньше, чем у противника, он удваивается")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            int myPower = battle.GetCharacter().GetPower();
            int enemyPower = battle.GetEnemyCharacter().GetPower();

            if (myPower < enemyPower)
            {
                controller.ChangePowerSafe(battle.GetCharacter(), myPower);
            }
        }

        
    }
}
