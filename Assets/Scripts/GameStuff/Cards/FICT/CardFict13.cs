using MetaInfo;

namespace GameStuff
{
    public class CardFict13 : Card
    {
        public CardFict13()
            : base(13, 30, CardType.NO_BATTLE, StuffClass.FICT, false,
                  "Вы делаете дополнительный ход")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.ChangePlayersAfterMoveFinished(false);
        }
    }
}