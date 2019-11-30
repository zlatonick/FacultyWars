using BoardStuff;
using GameStuff;
using MetaInfo;

namespace GameEngine
{
    public class EngineImpl : Engine
    {
        private StuffClass stuffClass;

        private CheckFactory checkFactory;

        private int lastCellPlacedOn;

        private PlayerInfo playerInfo;

        public EngineImpl(StuffClass stuffClass)
        {
            this.stuffClass = stuffClass;
            playerInfo = new EnginePlayerController();

            checkFactory = new CheckFactoryImpl();
            lastCellPlacedOn = 0;
        }

        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }

        public StuffClass GetStuffClass()
        {
            return stuffClass;
        }

        public Card MakeBattleMove()
        {
            return null;
        }

        public PlayerMove MakeMove(MatchController controller)
        {
            Check check = checkFactory.GetCheck(stuffClass, 1);
            Cell cell = controller.GetCellById(lastCellPlacedOn);
            lastCellPlacedOn++;

            return new PlayerMove(check, cell);
        }
    }
}