using MetaInfo;

namespace BoardStuff
{
    public class CharacterFactory
    {
        public Character CreateCharacter(StuffClass stuffClass, int power, Player player)
        {
            return new CharacterImpl(stuffClass, power, player);
        }
    }
}