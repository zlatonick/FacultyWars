using MetaInfo;

namespace GameStuff
{
    public class CheckFactoryImpl : CheckFactory
    {
        public Check GetCheck(StuffClass stuffClass, int level)
        {
            int power = 0;

            switch (stuffClass)
            {
                case StuffClass.IASA:
                    if (level == 0) power = 40;
                    else if (level == 1) power = 60;
                    else if (level == 2) power = 70;
                    break;

                case StuffClass.FICT:
                    if (level == 0) power = 20;
                    else if (level == 1) power = 30;
                    else if (level == 2) power = 50;
                    break;

                case StuffClass.FPM:
                    if (level == 0) power = 30;
                    else if (level == 1) power = 50;
                    else if (level == 2) power = 60;
                    break;
            }

            return new CheckImpl(level, power, stuffClass);
        }
    }
}