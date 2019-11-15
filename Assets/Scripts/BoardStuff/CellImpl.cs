using UnityEngine;
using MetaInfo;
using System.Collections.Generic;

namespace BoardStuff
{    public class CellImpl : Cell
    {
        private int Id;

        private Dictionary<StuffClass, int> Effect;

        private CellState State;

        private bool Blocked;

        private GameObject CellObject;

        public CellImpl(int id, GameObject cellObject)
        {
            Id = id;
            Effect = GetRandomEffect();
            CellObject = cellObject;
            Blocked = false;
            State = CellState.CLOSED;

            CellObject.SetActive(true);
        }

        private Dictionary<StuffClass, int> GetRandomEffect()
        {
            Dictionary<StuffClass, int> result = new Dictionary<StuffClass, int>();

            return result;
        }

        public Dictionary<StuffClass, int> GetEffect()
        {
            return Effect;
        }

        public int getId()
        {
            return Id;
        }

        public CellState GetState()
        {
            return State;
        }

        public bool IsBlocked()
        {
            return Blocked;
        }

        public void SetBlock(bool blocked)
        {
            Blocked = blocked;
        }

        public void SetState(CellState state)
        {
            State = state;
        }

        public bool IsNeighbourTo(int cellId)
        {
            throw new System.NotImplementedException();
        }

        public void Redraw(Dictionary<StuffClass, int> effect)
        {
            throw new System.NotImplementedException();
        }
    }
}
