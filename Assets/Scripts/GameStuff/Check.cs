using MetaInfo;

namespace GameStuff
{
    public interface Check
    {
        int GetLevel();     // 0 - weak, 1 - middle, 2 - strong

        int GetPower();

        StuffClass GetStuffClass();
    }
}
