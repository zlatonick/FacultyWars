using BoardStuff;

namespace GameStuff
{
    public interface Chooser
    {
        Character ChooseCharacter();

        Cell ChooseCell();
    }
}
