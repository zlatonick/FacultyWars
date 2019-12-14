using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm6 : Card
    {
        public CardFpm6()
            : base(6, 50, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Каждый раз, когда вы разыгрываете карту в данном бою, ваш персонаж получает +10 к силе")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();

            controller.SetActionAfterCardIsPlayed(battle.GetPlayer(), card =>
            {
                controller.ChangePowerSafe(character, 10);
            });
        }

        
    }
}