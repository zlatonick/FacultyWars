using MetaInfo;

namespace BoardStuff
{
    public class CharacterFactory
    {
        private int charactersCreated;

        private BoardStuffManager boardStuffManager;

        public CharacterFactory(BoardStuffManager boardStuffManager)
        {
            this.boardStuffManager = boardStuffManager;
            charactersCreated = 0;
        }

        public Character CreateCharacter(StuffClass stuffClass, int level, int power, Player player)
        {
            return new CharacterImpl(charactersCreated++, stuffClass, level,
                power, player, boardStuffManager);
        }
    }
}