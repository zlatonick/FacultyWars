using MetaInfo;

namespace GameStuff
{
    public interface Card
    {
        CardType GetCardType();

        StuffClass GetStuffClass();

        string GetName();

        bool IsChoosing();

        void Choose(Chooser chooser);

        void act(Battle battle, MatchController controller);
    }
}
