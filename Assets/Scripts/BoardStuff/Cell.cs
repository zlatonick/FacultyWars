using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public interface Cell
    {
        int GetId();

        CellState GetState();

        void SetState(CellState state);

        bool IsBlocked();

        void SetBlock(bool blocked);

        void SetEffect(CellEffect effect);

        CellEffect GetEffect();
    }
}
