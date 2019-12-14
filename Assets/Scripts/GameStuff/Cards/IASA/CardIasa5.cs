using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardIasa5 : Card
    {
        public CardIasa5()
            : base(5, 60, CardType.SILVER, StuffClass.IASA, false,
                  "Устанавливает уровень силы вашего персонажа равным 80")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();
            controller.ChangePowerSafe(character, 80 - character.GetPower());
        }

        
    }
}
