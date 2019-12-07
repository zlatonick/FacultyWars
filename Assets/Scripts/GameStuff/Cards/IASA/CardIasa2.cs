using MetaInfo;

namespace GameStuff
{
    public class CardIasa2 : Card
    {
        public CardIasa2()
            : base(2, 60, CardType.SILVER, StuffClass.IASA, false,
                  "Если сила вашего персонажа 70 или больше, он получает + 30 к силе")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            if (battle.GetCharacter().GetPower() >= 70)
            {
                controller.ChangePowerSafe(battle.GetCharacter(), 30);
            }
        }

        public override void Choose(Chooser chooser) { }
    }
}
