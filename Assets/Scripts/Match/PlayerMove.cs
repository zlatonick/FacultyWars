using BoardStuff;
using GameStuff;

namespace GameEngine
{
    public class PlayerMove
    {
        public Check check;

        public Cell cell;

        public PlayerMove(Check check, Cell cell)
        {
            this.check = check;
            this.cell = cell;
        }
    }
}