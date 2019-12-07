using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardIasa8 : Card
    {
        public CardIasa8()
            : base(8, 50, CardType.SILVER, StuffClass.IASA, false,
                  "Если уровень силы вашего персонажа меньше первоначального, он получает +30 к силе") { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();

            if (character.GetPower() < character.GetStartPower())
            {
                controller.ChangePowerSafe(character, 30);
            }
        }

        public override void Choose(Chooser chooser) { }
    }
}
