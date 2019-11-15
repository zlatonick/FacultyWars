using BoardStuff;
using GameStuff;
using MetaInfo;

namespace Match
{
    public class BattleImpl : Battle
    {
        Cell cell;

        Player player;

        Character character;

        Player opponent;

        Character enemyCharacter;

        public BattleImpl(Cell cell, Player player, Character character,
            Player opponent, Character enemyCharacter)
        {
            this.cell = cell;
            this.player = player;
            this.character = character;
            this.opponent = opponent;
            this.enemyCharacter = enemyCharacter;
        }

        public Cell GetCell()
        {
            return cell;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public Character GetEnemyCharacter()
        {
            return enemyCharacter;
        }

        public Player GetEnemyPlayer()
        {
            return opponent;
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}