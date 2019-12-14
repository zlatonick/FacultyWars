using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm0 : Card
    {
        public CardFpm0()
            : base(0, 30, CardType.NEUTRAL, StuffClass.FPM, false,
                  "Действие текущей клетки поля отменяется")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.ChangeCellEffect(battle.GetCell(), new CellEffect());
        }

        
    }
}