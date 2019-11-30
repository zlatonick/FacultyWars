using MetaInfo;

namespace GameStuff
{
    public class CardIasa0 : Card
    {
        public CardIasa0()
            : base(0, 60, CardType.SILVER, StuffClass.IASA, false,
                  "+20 к силе вашего персонажа в бою") { }

        public override void Act(Battle battle, MatchController controller)
        {
            battle.GetCharacter().ChangePower(20);
        }

        public override void Choose(Chooser chooser) { }
    }
}
