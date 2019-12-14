using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm5 : Card
    {
        public CardFpm5()
            : base(5, 30, CardType.NEUTRAL, StuffClass.FPM, false,
                  "После боя клетка поля остается открытой")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.DontCloseCellAfterBattle(true);
        }

        
    }
}