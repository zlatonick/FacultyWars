using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFpm1 : Card
    {
        public CardFpm1()
            : base(1, 40, CardType.SILVER, StuffClass.FPM, false,
                  "Действие текущей клетки поля меняется на противоположное")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            Cell cell = battle.GetCell();
            controller.ChangeCellEffect(cell, CellEffect.GetInverted(cell.GetEffect()));
        }

        
    }
}