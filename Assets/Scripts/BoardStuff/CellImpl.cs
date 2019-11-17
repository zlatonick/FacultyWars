using System;
using MetaInfo;

namespace BoardStuff
{    public class CellImpl : Cell
    {
        private int id;

        private CellEffect effect;

        private CellState state;

        private bool blocked;

        private CellManager cellManager;

        public CellImpl(int id, CellManager cellManager)
        {
            this.id = id;
            this.cellManager = cellManager;

            effect = GetRandomEffect();
            blocked = false;
            state = CellState.CLOSED;
        }

        private static int GetRandomEffectPower(Random random)
        {
            if (random.Next(2) == 0)
            {
                return random.Next(1, 3) * 10;
            }
            else
            {
                return -random.Next(1, 3) * 10;
            }
        }

        private static CellEffect GetRandomEffect()
        {
            /*
             * Generating a random cell effect
             * 50% - cell has two effects
             * 40% - cell has one effect
             * 10% - cell has no effects
             * Every effect and its value is chosen randomly with equal probabilities
             */

            Random random = new Random();

            int effectsQuan = random.Next(10);

            if (effectsQuan < 5)
            {
                int stuffInt = random.Next(3);

                int[] numbers = new int[2];
                int index = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i != stuffInt)
                    {
                        numbers[index] = i;
                        index++;
                    }
                }

                int stuffInt2 = numbers[random.Next(2)];

                return new CellEffect((StuffClass)stuffInt, GetRandomEffectPower(random),
                    (StuffClass)stuffInt2, GetRandomEffectPower(random));
            }
            else if (effectsQuan < 9)
            {
                return new CellEffect((StuffClass)random.Next(3), GetRandomEffectPower(random));
            }
            else
            {
                return new CellEffect();
            }
        }

        public CellEffect GetEffect()
        {
            return effect;
        }

        public int getId()
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

        public void SetState(CellState newState)
        {
            if (state == CellState.CLOSED)
            {
                if (newState == CellState.BATTLED || newState == CellState.OPENED)
                {
                    cellManager.ChangeCellPrefab(id, true);
                }
            }
            else
            {
                state = newState;
            }
        }

        public void Redraw(CellEffect effect)
        {
            cellManager.RemoveEffect(id);
            
            if (effect.EffectsQuan == 1)
            {
                cellManager.SetEffect(id, effect.StuffClass, effect.Power);
            }
            else
            {
                cellManager.SetEffect(id, effect.StuffClass, effect.Power,
                    effect.StuffClass2, effect.Power2);
            }
        }
    }
}
