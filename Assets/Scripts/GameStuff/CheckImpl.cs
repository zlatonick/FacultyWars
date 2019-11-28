using MetaInfo;

namespace GameStuff
{
    public class CheckImpl : Check
    {
        private int level;      // 0 - weak, 1 - middle, 2 - strong

        private int power;

        private StuffClass stuffClass;

        public CheckImpl(int level, int power, StuffClass stuffClass)
        {
            this.level = level;
            this.power = power;
            this.stuffClass = stuffClass;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetPower()
        {
            return power;
        }

        public StuffClass GetStuffClass()
        {
            return stuffClass;
        }
    }
}