using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public static class CheckLevels
    {
        private static Dictionary<StuffClass, Dictionary<int, Check>> checkLevels;

        private static CheckFactory checkFactory;

        static CheckLevels()
        {
            checkFactory = new CheckFactoryImpl();

            checkLevels = new Dictionary<StuffClass, Dictionary<int, Check>>();

            checkLevels.Add(StuffClass.IASA, new Dictionary<int, Check>());
            checkLevels.Add(StuffClass.FICT, new Dictionary<int, Check>());
            checkLevels.Add(StuffClass.FPM, new Dictionary<int, Check>());

            for (int i = 0; i < 3; i++)
            {
                checkLevels[StuffClass.IASA].Add(i, checkFactory.GetCheck(StuffClass.IASA, i));
                checkLevels[StuffClass.FICT].Add(i, checkFactory.GetCheck(StuffClass.FICT, i));
                checkLevels[StuffClass.FPM].Add(i, checkFactory.GetCheck(StuffClass.FPM, i));
            }
        }

        public static Dictionary<int, Check> GetCheckLevels(StuffClass stuffClass)
        {
            return checkLevels[stuffClass];
        }
    }
}