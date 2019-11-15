using BoardStuff;
using GameEngine;
using GameStuff;
using MetaInfo;
using Preparing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match
{   
    public class MatchControllerImpl : MonoBehaviour, MatchController
    {
        // Match info

        public Board board;

        Dictionary<Player, PlayerInfo> playersInfo;

        // Players

        Player currPlayer;

        Player currOpponent;

        // Battle info

        bool isBattleNow;

        Dictionary<Player, List<Battle>> battles;

        Dictionary<Player, BattleStatus> battleStatus;

        // Factories

        CharacterFactory characterFactory;

        CheckFactory checkFactory;

        // Start is called before the first frame update
        void Start()
        {
            // Creating the players
            Player mainPlayer = new Player(0, StuffPack.stuffClass);

            Engine engine = EngineCreator.CreateEngine();   // Versus AI
            Player opponent = new Player(1, engine.GetStuffClass());

            // Setting the players info
            playersInfo = new Dictionary<Player, PlayerInfo>();

            playersInfo.Add(mainPlayer, new PlayerController(
                StuffPack.stuffClass, StuffPack.cards, StuffPack.checks));

            playersInfo.Add(opponent, new PlayerController(
                engine.GetStuffClass(), engine.GetCards(), engine.GetChecks()));

            // Choosing the first player
            System.Random random = new System.Random();

            if (random.Next(0, 2) == 0)
            {
                currPlayer = mainPlayer;
                currOpponent = opponent;
            }
            else
            {
                currPlayer = opponent;
                currOpponent = mainPlayer;
            }

            // Creating the factories
            characterFactory = new CharacterFactory();
            checkFactory = new CheckFactoryImpl();

            // Creating the battles
            battles = new Dictionary<Player, List<Battle>>();

            battles.Add(mainPlayer, new List<Battle>());
            battles.Add(opponent, new List<Battle>());

            isBattleNow = false;

            battleStatus = new Dictionary<Player, BattleStatus>();

            battleStatus.Add(mainPlayer, BattleStatus.NO_BATTLE);
            battleStatus.Add(opponent, BattleStatus.NO_BATTLE);
        }

        private void ChangeMove()
        {
            Player temp = currPlayer;

            currPlayer = currOpponent;
            currOpponent = temp;
        }

        public void PlaceCheck(Check check, Cell cell)
        {
            // Spawning the character
            Character newCharacter = characterFactory.CreateCharacter(
                check.GetStuffClass(), check.GetPower(), currPlayer);

            board.SpawnCharacter(newCharacter, cell);

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Checking if the battle is already going
            if (characters.Count > 2)
            {
                // Finding all the enemies
                List<Character> enemies = new List<Character>();

                foreach (Character character in characters)
                {
                    if (character.GetPlayer() == currOpponent)
                    {
                        enemies.Add(character);
                    }
                }

                // Starting the battles
                foreach (Character enemy in enemies)
                {
                    battles[currPlayer].Add(new BattleImpl(
                        cell, currPlayer, newCharacter, currOpponent, enemy));
                    battles[currOpponent].Add(new BattleImpl(
                        cell, currOpponent, enemy, currPlayer, newCharacter));
                }
            }
            // If the first battle was started - start the battle
            else if (characters.Count == 2)
            {
                StartBattle(cell);
            }

            ChangeMove();
        }

        // Starting the battle with two characters on cell
        private void StartBattle(Cell cell)
        {
            // Getting the characters
            List<Character> characters = board.GetCharactersOnCell(cell);

            if (characters.Count != 2)
            {
                throw new InvalidOperationException(
                    "Incorrect battle creation: more than 2 characters");
            }

            bool firstIsPlayer = characters[0].GetPlayer() == currPlayer;

            Character playerChr = firstIsPlayer ? characters[0] : characters[1];
            Character opponentChr = firstIsPlayer ? characters[1] : characters[0];

            // Creating the battles
            battles[currPlayer].Add(new BattleImpl(
                cell, currPlayer, playerChr, currOpponent, opponentChr));

            battles[currOpponent].Add(new BattleImpl(
                cell, currOpponent, opponentChr, currPlayer, playerChr));

            // Starting the battle
            isBattleNow = true;

            battleStatus[currPlayer] = BattleStatus.NO_CARDS_PLAYED;
            battleStatus[currOpponent] = BattleStatus.NO_CARDS_PLAYED;

            OpenCell(cell);

            board.StartBattle(cell);
        }

        public void ChangeCellEffect(Cell cell, Dictionary<StuffClass, int> effect)
        {
            // Redrawing the cell
            Dictionary<StuffClass, int> oldEffect = cell.GetEffect();

            cell.Redraw(effect);

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Removing the old effect
            foreach (var pair in oldEffect)
            {
                foreach (Character character in characters)
                {
                    if (character.GetStuffClass() == pair.Key)
                    {
                        character.ChangePower(-pair.Value);
                    }
                }
            }

            // Adding the new effect
            foreach (var pair in effect)
            {
                foreach (Character character in characters)
                {
                    if (character.GetStuffClass() == pair.Key)
                    {
                        character.ChangePower(pair.Value);
                    }
                }
            }
        }

        public void FinishBattle(Player winner)
        {
            Cell cell = battles[currPlayer][0].GetCell();

            // Drawing the battle finishing
            board.FinishBattle(cell, winner);

            // Removing the characters
            HashSet<Character> winnerChars = new HashSet<Character>();
            HashSet<Character> opponentChars = new HashSet<Character>();

            foreach (Battle battle in battles[winner == null ? currPlayer : winner])
            {
                winnerChars.Add(battle.GetCharacter());
                opponentChars.Add(battle.GetEnemyCharacter());
            }

            if (winner == null)
            {
                // Draw. Returning all the characters to hands
                foreach (Character character in winnerChars)
                {
                    board.ReturnCharacter(character);
                    playersInfo[currPlayer].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetStartPower()));
                }

                foreach (Character character in opponentChars)
                {
                    board.ReturnCharacter(character);
                    playersInfo[currOpponent].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetStartPower()));
                }
            }
            else
            {
                // Returning all the winner characters to his hand
                foreach (Character character in winnerChars)
                {
                    board.ReturnCharacter(character);
                    playersInfo[winner].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetStartPower()));
                }

                // Destroying all the loser characters
                foreach (Character character in opponentChars)
                {
                    board.DestroyCharacter(character);
                }
            }

            // Closing the cell
            board.RemoveCell(cell);

            // Finishing the battles
            isBattleNow = false;

            foreach (var pair in battles)
            {
                pair.Value.Clear();
            }

            battleStatus[currPlayer] = BattleStatus.NO_BATTLE;
            battleStatus[currOpponent] = BattleStatus.NO_BATTLE;
        }

        public void PlayCard(Card card)
        {
            if (isBattleNow)
            {
                foreach (Battle battle in battles[currPlayer])
                {
                    card.Act(battle, this);
                }

                // Changing the battle status
                if (card.GetCardType() == CardType.SILVER)
                {
                    if (battleStatus[currPlayer] == BattleStatus.NO_CARDS_PLAYED)
                    {
                        battleStatus[currPlayer] = BattleStatus.ONE_CARD_PLAYED;
                    }
                    else if (battleStatus[currPlayer] == BattleStatus.ONE_CARD_PLAYED)
                    {
                        battleStatus[currPlayer] = BattleStatus.TWO_CARDS_PLAYED;
                    }
                }
                else if (card.GetCardType() == CardType.GOLD)
                {
                    battleStatus[currPlayer] = BattleStatus.TWO_CARDS_PLAYED;
                }

                ChangeMove();
            }
            else
            {
                card.Act(null, this);
            }
        }

        public List<Cell> GetAllCells()
        {
            return board.GetAllCells();
        }

        public List<Character> GetAllCharacters()
        {
            return board.GetAllCharacters();
        }

        public PlayerInfo GetPlayerInfo(Player player)
        {
            return playersInfo[player];
        }

        public bool IsBattleNow()
        {
            return isBattleNow;
        }

        public void MoveCharacter(Character character, Cell cell)
        {
            throw new NotImplementedException();
        }

        public void OpenCell(Cell cell)
        {
            cell.SetState(CellState.BATTLED);

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Adding the effect to characters
            foreach (var pair in cell.GetEffect())
            {
                foreach (Character character in characters)
                {
                    if (character.GetStuffClass() == pair.Key)
                    {
                        character.ChangePower(pair.Value);
                    }
                }
            }
        }

        public void SetAfterBattleAction(Action<Player> action)
        {
            throw new NotImplementedException();
        }

        public void SetAfterNTurnsAction(Action action, int n)
        {
            throw new NotImplementedException();
        }

        public void SetNextTurnPlayer(Player player)
        {
            throw new NotImplementedException();
        }


    }
}
