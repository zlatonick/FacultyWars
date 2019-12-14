using MetaInfo;

namespace GameStuff
{
    public class CardAll0 : Card
    {
        public CardAll0(StuffClass stuffClass)
            : base(0, 0, CardType.NEUTRAL, stuffClass, false,
                  "+10 к силе вашего персонажа в бою")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.ChangePowerSafe(battle.GetCharacter(), 10);
        }
    }
}