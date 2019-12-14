using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFict14 : Card
    {
        public CardFict14()
            : base(14, 60, CardType.GOLD, StuffClass.FICT, false,
                  "Уровни силы вашего персонажа и противника меняются местами")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();
            Character enemy = battle.GetEnemyCharacter();

            int myPower = character.GetPower();
            int enemyPower = enemy.GetPower();

            controller.ChangePowerSafe(character, enemyPower - myPower);
            controller.ChangePowerSafe(enemy, myPower - enemyPower);
        }
    }
}