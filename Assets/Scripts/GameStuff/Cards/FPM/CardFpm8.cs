using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm8 : Card
    {
        public CardFpm8()
            : base(8, 50, CardType.SILVER, StuffClass.FPM, false,
                  "+10 к силе вашему персонажу в бою. Если уровень силы персонажа " +
                  "меньше 60, карта возвращается к руку")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());
            Character character = battle.GetCharacter();

            controller.ChangePowerSafe(character, 10);

            if (character.GetPower() < 60)
            {
                playerInfo.AddCardToHand(new CardFpm8());
            }
        }

        
    }
}