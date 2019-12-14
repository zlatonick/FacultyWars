using GameStuff;
using MetaInfo;

namespace GameEngine
{
    public interface Engine
    {
        void SetMatchController(MatchController controller);

        StuffClass GetStuffClass();

        PlayerInfo GetPlayerInfo();

        PlayerMove MakeMove();

        Card MakeBattleMove();
    }
}
