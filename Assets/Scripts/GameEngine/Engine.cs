using GameStuff;
using MetaInfo;

namespace GameEngine
{
    public interface Engine
    {
        StuffClass GetStuffClass();

        PlayerInfo GetPlayerInfo();

        PlayerMove MakeMove(MatchController controller);

        Card MakeBattleMove();
    }
}
