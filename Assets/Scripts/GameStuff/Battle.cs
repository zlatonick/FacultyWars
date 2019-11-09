using BoardStuff;
using MetaInfo;

namespace GameStuff
{
    public interface Battle
    {
        Player GetPlayer();

        Player GetEnemyPlayer();

        Cell GetCell();

        Character GetCharacter();

        Character GetEnemyCharacter();
    }
}
