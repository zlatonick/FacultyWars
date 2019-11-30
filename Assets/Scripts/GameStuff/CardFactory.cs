using MetaInfo;

namespace GameStuff
{
    public interface CardFactory
    {
        Card GetCard(StuffClass stuffClass, int id);
    }
}
