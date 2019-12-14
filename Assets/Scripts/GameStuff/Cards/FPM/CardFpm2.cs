using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm2 : Card
    {
        public CardFpm2()
            : base(2, 40, CardType.SILVER, StuffClass.FPM, false,
                  "Если текущая клетка поля взаимодействует с вашим классом, вы получаете +30 к силе")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Character character = battle.GetCharacter();

            if (battle.GetCell().GetEffect().CheckEffect(character.GetStuffClass()))
            {
                controller.ChangePowerSafe(character, 30);
            }
        }

        
    }
}