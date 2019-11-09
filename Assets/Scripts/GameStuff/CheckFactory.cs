using MetaInfo;

namespace GameStuff
{
    public interface CheckFactory
    {
        Check GetCheck(StuffClass stuffClass, int power);
    }
}
