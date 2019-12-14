using MetaInfo;
using System;
using System.Collections.Generic;

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
                if (id == 0) return new CardFpm0();
                if (id == 1) return new CardFpm1();
                if (id == 2) return new CardFpm2();
                if (id == 3) return new CardFpm3();
                if (id == 4) return new CardFpm4();
                if (id == 5) return new CardFpm5();
                if (id == 6) return new CardFpm6();
                if (id == 7) return new CardFpm7();
                if (id == 8) return new CardFpm8();
                if (id == 9) return new CardFpm9();
                if (id == 10) return new CardFpm10();
                if (id == 11) return new CardFpm11();
                if (id == 12) return new CardFpm12();
                if (id == 13) return new CardFpm13();
                if (id == 14) return new CardFpm14();
                if (id == 15) return new CardFpm15();
                if (id == 16) return new CardFpm16();
                if (id == 17) return new CardFpm17();
                if (id == 18) return new CardFpm18();
            }

            return null;
        }

        public Card GetRandomCard(StuffClass stuffClass, CardType cardType)
        {
            Random random = new Random();

            List<int> cardsRange = null;

            if (stuffClass == StuffClass.IASA)
            {
                if (cardType == CardType.NO_BATTLE || cardType == CardType.NEUTRAL)
                    return null;
                else if (cardType == CardType.SILVER)
                    cardsRange = new List<int> { 0, 1, 2, 5, 9, 10 };
                else
                    cardsRange = new List<int> { 3, 4, 6, 7, 8 };
            }
            else if (stuffClass == StuffClass.FICT)
            {
                if (cardType == CardType.NO_BATTLE)
                    cardsRange = new List<int> { 6, 7, 11, 13 };
                else if (cardType == CardType.NEUTRAL)
                    cardsRange = new List<int> { 5, 8, 16 };
                else if (cardType == CardType.SILVER)
                    cardsRange = new List<int> { 0, 1, 3, 4, 9, 10, 12 };
                else
                    cardsRange = new List<int> { 2, 14, 15 };
            }
            else if (stuffClass == StuffClass.FPM)
            {
                if (cardType == CardType.NO_BATTLE)
                    cardsRange = new List<int> { 9, 13, 14 };
                else if (cardType == CardType.NEUTRAL)
                    cardsRange = new List<int> { 0, 3, 5, 6, 15, 18 };
                else if (cardType == CardType.SILVER)
                    cardsRange = new List<int> { 1, 2, 7, 8, 10, 11, 12, 16 };
                else
                    cardsRange = new List<int> { 4, 17 };
            }

            return GetCard(stuffClass, cardsRange[random.Next(cardsRange.Count)]);
        }
    }
}