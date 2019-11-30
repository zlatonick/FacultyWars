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
            }

            return null;
        }
    }
}