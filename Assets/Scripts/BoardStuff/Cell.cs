using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public interface Cell
    {
        int getId();

        CellState GetState();

        void SetState(CellState state);

        bool IsBlocked();

        void SetBlock(bool blocked);

        Dictionary<StuffClass, int> GetEffect();

        void Redraw(Dictionary<StuffClass, int> effect);

        bool IsNeighbourTo(int cellId);
    }
}
