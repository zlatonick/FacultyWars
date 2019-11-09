using MetaInfo;

namespace GameStuff
{
    public interface Check
    {
        int GetPower();

        StuffClass GetStuffClass();

        void setPower(int newPower);
    }
}
