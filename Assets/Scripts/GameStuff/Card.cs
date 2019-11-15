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

        void Act(Battle battle, MatchController controller);
    }
}
