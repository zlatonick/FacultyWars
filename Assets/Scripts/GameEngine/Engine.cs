using GameStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameEngine
{
    public interface Engine
    {
        StuffClass GetStuffClass();

        List<Card> GetCards();

        List<Check> GetChecks();

        PlayerMove MakeMove();

        Card MakeBattleMove();
    }
}
