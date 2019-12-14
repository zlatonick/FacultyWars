using MetaInfo;

namespace GameStuff
{
    public class CardFpm13 : Card
    {
        public CardFpm13()
            : base(13, 40, CardType.NO_BATTLE, StuffClass.FPM, false,
                  "Вы кладете себе в руку 3 нейтральных карты «+10 к силе»")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            playerInfo.AddCardToHand(new CardAll0(StuffClass.FPM));
            playerInfo.AddCardToHand(new CardAll0(StuffClass.FPM));
            playerInfo.AddCardToHand(new CardAll0(StuffClass.FPM));
        }

        
    }
}