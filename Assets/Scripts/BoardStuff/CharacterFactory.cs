using MetaInfo;

namespace BoardStuff
{
    public class CharacterFactory
    {
        private int charactersCreated;

        private CharacterManager characterManager;

        public CharacterFactory(CharacterManager characterManager)
        {
            this.characterManager = characterManager;
            charactersCreated = 0;
        }

        public Character CreateCharacter(StuffClass stuffClass, int power, Player player)
        {
            return new CharacterImpl(charactersCreated++, stuffClass,
                power, player, characterManager);
        }
    }
}