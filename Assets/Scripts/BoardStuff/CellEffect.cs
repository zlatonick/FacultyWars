
using MetaInfo;

namespace BoardStuff
{
    public class CellEffect
    {
        int effectsQuan;

        StuffClass stuffClass;

        int power;

        StuffClass stuffClass2;

        int power2;

        public CellEffect()
        {
            this.effectsQuan = 0;
        }

        public CellEffect(StuffClass stuffClass, int power)
        {
            this.effectsQuan = 1;
            this.stuffClass = stuffClass;
            this.power = power;
        }

        public CellEffect(StuffClass stuffClass, int power,
            StuffClass stuffClass2, int power2)
        {
            this.effectsQuan = 2;
            this.stuffClass = stuffClass;
            this.power = power;
            this.stuffClass2 = stuffClass2;
            this.power2 = power2;
        }

        public int EffectsQuan { get => effectsQuan; set => effectsQuan = value; }
        public StuffClass StuffClass { get => stuffClass; set => stuffClass = value; }
        public int Power { get => power; set => power = value; }
        public StuffClass StuffClass2 { get => stuffClass2; set => stuffClass2 = value; }
        public int Power2 { get => power2; set => power2 = value; }

        public bool CheckEffect(StuffClass checkClass)
        {
            if (effectsQuan == 0)
            {
                return false;
            }
            else if (effectsQuan == 1)
            {
                return stuffClass == checkClass;
            }
            else
            {
                return stuffClass == checkClass || stuffClass2 == checkClass;
            }
        }

        public int GetStuffClassPower(StuffClass stClass)
        {
            if (stClass == stuffClass)
            {
                return power;
            }
            else if (stClass == stuffClass2)
            {
                return power2;
            }
            else
            {
                return 0;
            }
        }
    }
}