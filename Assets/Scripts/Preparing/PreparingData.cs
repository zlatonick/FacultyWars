using System;
using MetaInfo;

namespace Preparing
{
    public class PreparingData
    {
        public static ShopItem[] shopItems = new[]
        {
            new ShopItem() {id = 0, itemTitle = "+ 10 to Power "},
            new ShopItem() {id = 1, itemTitle = "+ 20 to Power "},
            new ShopItem() {id = 2, itemTitle = "+ 30 to Power "},
            new ShopItem() {id = 3, itemTitle = "+ 40 to Power "},
            new ShopItem() {id = 4, itemTitle = "+ 50 to Power "},
            new ShopItem() {id = 5, itemTitle = "+ 60 to Power "},
            new ShopItem() {id = 6, itemTitle = "+ 70 to Power "},
            new ShopItem() {id = 7, itemTitle = "+ 80 to Power "},
            new ShopItem() {id = 8, itemTitle = "+ 90 to Power "},
            new ShopItem() {id = 9, itemTitle = "+ 100 to Power "}
        };

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