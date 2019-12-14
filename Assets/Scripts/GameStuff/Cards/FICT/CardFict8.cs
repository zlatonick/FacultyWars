using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public class CardFict8 : Card
    {
        private CheckFactory checkFactory;

        public CardFict8()
            : base(8, 30, CardType.NEUTRAL, StuffClass.FICT, false,
                  "В случае проигрыша ваш персонаж возвращается в руку")
        {
            checkFactory = new CheckFactoryImpl();
        }

        public override void Act(Battle battle, MatchController controller)
        {
            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());
            Character character = battle.GetCharacter();

            controller.SetAfterBattleAction(winner =>
            {
                if (winner != null && winner == battle.GetEnemyPlayer())
                {
                    playerInfo.AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetLevel()));
                }
            });
        }

        
    }
}