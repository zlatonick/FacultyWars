using GameStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameEngine
{
    public interface Engine
    {
        StuffClass GetStuffClass();

        PlayerInfo GetPlayerInfo();

        PlayerMove MakeMove();

        Card MakeBattleMove();
    }
}
