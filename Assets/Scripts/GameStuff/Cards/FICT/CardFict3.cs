using MetaInfo;

namespace GameStuff
{
    public class CardFict3 : Card
    {
        public CardFict3()
            : base(3, 40, CardType.SILVER, StuffClass.FICT, false,
                  "+10 к силе вашему персонажу в бою за каждого вашего персонажа, погибшего в матче")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            controller.ChangePowerSafe(battle.GetCharacter(),
                playerInfo.GetChecksDead().Count * 10);
        }

        public override void Choose(Chooser chooser) { }
    }
}