using BoardStuff;

namespace GameStuff
{
    public interface Chooser
    {
        Character ChooseCharacter(string message);

        Cell ChooseCell(string message);

        Check ChooseCheck(string message);
    }
}
