using MetaInfo;

namespace GameStuff
{
    public class CardFactoryImpl : CardFactory
    {
        public Card GetCard(StuffClass stuffClass, int id)
        {
            if (stuffClass == StuffClass.IASA)
            {
                if (id == 0) return new CardIasa0();
                if (id == 1) return new CardIasa1();
                if (id == 2) return new CardIasa2();
                if (id == 3) return new CardIasa3();
                if (id == 4) return new CardIasa4();
                if (id == 5) return new CardIasa5();
                if (id == 6) return new CardIasa6();
                if (id == 7) return new CardIasa7();
                if (id == 8) return new CardIasa8();
                if (id == 9) return new CardIasa9();
            }
            else if (stuffClass == StuffClass.FICT)
            {
                if (id == 0) return new CardFict0();
                if (id == 1) return new CardFict1();
                if (id == 2) return new CardFict2();
                if (id == 3) return new CardFict3();
                if (id == 4) return new CardFict4();
                if (id == 5) return new CardFict5();
                if (id == 6) return new CardFict6();
                if (id == 7) return new CardFict7();
                if (id == 8) return new CardFict8();
                if (id == 9) return new CardFict9();
                if (id == 10) return new CardFict10();
                if (id == 11) return new CardFict11();
                if (id == 12) return new CardFict12();
                if (id == 13) return new CardFict13();
                if (id == 14) return new CardFict14();
                if (id == 15) return new CardFict15();
                if (id == 16) return new CardFict16();
            }
            else if (stuffClass == StuffClass.FPM)
            {

            }

            return null;
        }
    }
}