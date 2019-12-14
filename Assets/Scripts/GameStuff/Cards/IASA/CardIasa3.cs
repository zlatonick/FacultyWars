using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardIasa3 : Card
    {
        public CardIasa3()
            : base(3, 80, CardType.GOLD, StuffClass.IASA, false,
                  "Устанавливает уровни силы персонажей равными первоначальным уровням")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();
            Character enemyCharacter = battle.GetEnemyCharacter();

            controller.ChangePowerSafe(character, character.GetStartPower() - character.GetPower());
            controller.ChangePowerSafe(enemyCharacter,
                enemyCharacter.GetStartPower() - enemyCharacter.GetPower());
        }

        
    }
}
