using MetaInfo;

namespace GameStuff
{
    public class CardIasa4 : Card
    {
        public CardIasa4()
            : base(4, 90, CardType.GOLD, StuffClass.IASA, false,
                  "Уравнивает силы противников (по уровню сильнейшего) " +
                  "и добавляет 20 к силе вашего персонажа")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            int myPower = battle.GetCharacter().GetPower();
            int enemyPower = battle.GetEnemyCharacter().GetPower();

            int maxPower = myPower > enemyPower ? myPower : enemyPower;

            controller.ChangePowerSafe(battle.GetCharacter(), maxPower - myPower);
            controller.ChangePowerSafe(battle.GetEnemyCharacter(), maxPower - enemyPower);
            controller.ChangePowerSafe(battle.GetCharacter(), 20);
        }

        public override void Choose(Chooser chooser) { }
    }
}
