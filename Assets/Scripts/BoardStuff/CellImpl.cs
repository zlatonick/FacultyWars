using System;
using MetaInfo;

namespace BoardStuff
{    public class CellImpl : Cell
    {
        private int id;

        private CellEffect effect;

        private CellState state;

        private bool blocked;

        public CellImpl(int id)
        {
            this.id = id;

            blocked = false;
            state = CellState.CLOSED;
        }    

        public CellEffect GetEffect()
        {
            return effect;
        }

        public int GetId()
        {
            return id;
        }

        public CellState GetState()
        {
            return state;
        }

        public bool IsBlocked()
        {
            return blocked;
        }

        public void SetBlock(bool blocked)
        {
            this.blocked = blocked;
        }

        public void SetEffect(CellEffect effect)
        {
            this.effect = effect;
        }

        public void SetState(CellState newState)
        {
            state = newState;
        }
    }
}
