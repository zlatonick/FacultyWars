using MetaInfo;

namespace GameStuff
{
    public interface CardFactory
    {
        Card GetCard(StuffClass stuffClass, int id);

        Card GetRandomCard(StuffClass stuffClass, CardType cardType);
    }
}
