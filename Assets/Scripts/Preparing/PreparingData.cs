using MetaInfo;

namespace Preparing
{
    public class PreparingData
    {
        public static int cardCount = 10;

        public static ShopCheck[] shopChecksIasa = new[]
        {
            new ShopCheck(40, 80, StuffClass.IASA),
            new ShopCheck(60, 110, StuffClass.IASA),
            new ShopCheck(70, 130, StuffClass.IASA)
        };

        public static ShopCheck[] shopChecksFict = new[]
        {
            new ShopCheck(20, 50, StuffClass.FICT),
            new ShopCheck(30, 70, StuffClass.FICT),
            new ShopCheck(50, 100, StuffClass.FICT)
        };

        public static ShopCheck[] shopChecksFpm = new[]
        {
            new ShopCheck(30, 100, StuffClass.FPM),
            new ShopCheck(50, 130, StuffClass.FPM),
            new ShopCheck(60, 150, StuffClass.FPM)
        };
    }

    public class ShopCheck
    {
        public readonly int power;
        public readonly int price;
        public readonly StuffClass stuffClass;

        public ShopCheck(int power, int price, StuffClass stuffClass)
        {
            this.power = power;
            this.price = price;
            this.stuffClass = stuffClass;
        }
    }
}