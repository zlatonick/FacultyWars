using BoardStuff;
using System;

namespace GameStuff
{
    public interface Chooser
    {
        void ChooseCharacter(string message, Action<Character> action);

        void ChooseCell(string message, Action<Cell> action);

        void ChooseCheck(string message, Action<Check> action);
    }
}
