using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardIasa8 : Card
    {
        public CardIasa8()
            : base(8, 90, CardType.GOLD, StuffClass.IASA, false,
                  "Вы получаете бонус к силе, равный количеству силы, " +
                  "полученной вашим противником в этом бою ") { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();
            Character opponent = battle.GetEnemyCharacter();

            int changeBy = opponent.GetPower() - opponent.GetStartPower();

            controller.ChangePowerSafe(character, changeBy);
        }

        
    }
}
