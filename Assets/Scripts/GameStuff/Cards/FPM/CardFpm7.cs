using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm7 : Card
    {
        public CardFpm7()
            : base(7, 50, CardType.SILVER, StuffClass.FPM, false,
                  "Ничья в бою. Клетка остается открытой")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.DontCloseCellAfterBattle(true);
            controller.FinishBattle(null);
        }

        
    }
}