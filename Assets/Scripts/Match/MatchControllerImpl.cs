using BoardStuff;
using GameStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace Match
{   
    public class MatchControllerImpl : MatchController
    {
        // Match info

        private Board board;

        Dictionary<Player, PlayerInfo> playersInfo;

        // Players

        Player currPlayer;

        Player currOpponent;

        bool changePlayersAfterMoveFinished;

        // Battle info

        bool isBattleNow;

        Dictionary<Player, List<Battle>> battles;

        Dictionary<Player, List<Character>> battleCharacters;

        Dictionary<Player, BattleStatus> battleStatuses;

        // Factories

        CheckFactory checkFactory;

        public MatchControllerImpl(Board board, Player player1, PlayerInfo playerInfo1,
            Player player2, PlayerInfo playerInfo2)
        {
            this.board = board;

            // Setting the players info
            playersInfo = new Dictionary<Player, PlayerInfo>();

            playersInfo.Add(player1, playerInfo1);
            playersInfo.Add(player2, playerInfo2);

            // Choosing the first player
            System.Random random = new System.Random();

            if (random.Next(0, 2) == 0)
            {
                currPlayer = player1;
                currOpponent = player2;
            }
            else
            {
                currPlayer = player2;
                currOpponent = player1;
            }

            changePlayersAfterMoveFinished = true;

            // Creating the factories
            checkFactory = new CheckFactoryImpl();

            // Creating the battles
            battles = new Dictionary<Player, List<Battle>>();

            battles.Add(player1, new List<Battle>());
            battles.Add(player2, new List<Battle>());

            battleCharacters = new Dictionary<Player, List<Character>>();

            battleCharacters.Add(player1, new List<Character>());
            battleCharacters.Add(player2, new List<Character>());

            isBattleNow = false;

            battleStatuses = new Dictionary<Player, BattleStatus>();

            battleStatuses.Add(player1, BattleStatus.NO_BATTLE);
            battleStatuses.Add(player2, BattleStatus.NO_BATTLE);
        }

        private void ChangeMove()
        {
            if (changePlayersAfterMoveFinished)
            {
                Player temp = currPlayer;

                currPlayer = currOpponent;
                currOpponent = temp;
            }
            else
            {
                changePlayersAfterMoveFinished = true;
            }
        }

        private void ChangePowerSafe(Character character, int changeBy)
        {
            int currPower = character.GetPower();

            if (currPower + changeBy > 0)
            {
                character.ChangePower(changeBy);
            }
            else
            {
                character.ChangePower(-currPower);
                board.DestroyCharacter(character);
            }
        }

        public void PlaceCheck(Check check, Cell cell)
        {
            // Spawning the character
            Character newCharacter = board.SpawnCharacter(
                check.GetStuffClass(), check.GetPower(), currPlayer, cell);

            if (cell.GetState() == CellState.OPENED ||
                cell.GetState() == CellState.BATTLED)
            {
                AddCellEffectToCharacter(newCharacter, cell.GetEffect());
            }

            CheckBattlesWithNewCharacter(newCharacter, cell);

            ChangeMove();
        }

        private void CheckBattlesWithNewCharacter(Character newCharacter, Cell cell)
        {
            Player charPlayer = newCharacter.GetPlayer();
            Player charOpponent = charPlayer == currPlayer ? currOpponent : currPlayer;

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Finding all the enemies
            List<Character> enemies = new List<Character>();

            foreach (Character character in characters)
            {
                if (character.GetPlayer() == charOpponent)
                {
                    enemies.Add(character);
                }
            }

            // Checking if there are some enemies on the cell
            if (enemies.Count > 0)
            {
                // Checking if battle is already going
                if (characters.Count > enemies.Count)
                {
                    // Adding character to the battle
                    battleCharacters[charPlayer].Add(newCharacter);

                    // Starting the battles
                    foreach (Character enemy in enemies)
                    {
                        battles[charPlayer].Add(new BattleImpl(
                            cell, charPlayer, newCharacter, charOpponent, enemy));
                        battles[charOpponent].Add(new BattleImpl(
                            cell, charOpponent, enemy, charPlayer, newCharacter));
                    }
                }
                // If the first battle was started - start the battle
                else
                {
                    // Adding the characters to battle
                    battleCharacters[charPlayer].Add(newCharacter);
                    battleCharacters[charOpponent].AddRange(enemies);

                    // Creating the battles
                    foreach (Character enemy in enemies)
                    {
                        battles[charPlayer].Add(new BattleImpl(
                            cell, charPlayer, newCharacter, charOpponent, enemy));

                        battles[charOpponent].Add(new BattleImpl(
                            cell, charOpponent, enemy, charPlayer, newCharacter));
                    }

                    // Starting the battle
                    isBattleNow = true;

                    battleStatuses[charPlayer] = BattleStatus.NO_CARDS_PLAYED;
                    battleStatuses[charOpponent] = BattleStatus.NO_CARDS_PLAYED;

                    OpenCell(cell, CellState.BATTLED);

                    board.StartBattle(cell);
                }
            }
        }

        private void RemoveCellEffectFromCharacter(Character character, CellEffect effect)
        {
            if (effect.CheckEffect(character.GetStuffClass()))
            {
                ChangePowerSafe(character,
                    -effect.GetStuffClassPower(character.GetStuffClass()));
            }
        }

        private void AddCellEffectToCharacter(Character character, CellEffect effect)
        {
            if (effect.CheckEffect(character.GetStuffClass()))
            {
                ChangePowerSafe(character,
                    effect.GetStuffClassPower(character.GetStuffClass()));
            }
        }

        public void ChangeCellEffect(Cell cell, CellEffect effect)
        {
            // Redrawing the cell
            CellEffect oldEffect = cell.GetEffect();

            cell.Redraw(effect);

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Removing the old effect
            foreach (Character character in characters)
            {
                RemoveCellEffectFromCharacter(character, oldEffect);
            }

            // Adding the new effect
            foreach(Character character in characters)
            {
                AddCellEffectToCharacter(character, effect);
            }
        }

        public void FinishBattle(Player winner)
        {
            Cell cell = battles[currPlayer][0].GetCell();

            // Drawing the battle finishing
            board.FinishBattle(cell, winner);

            // Removing the characters
            if (winner == null)
            {
                // Draw. Returning all the characters to hands
                foreach (Character character in battleCharacters[currPlayer])
                {
                    board.ReturnCharacter(character);
                    playersInfo[currPlayer].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetStartPower()));
                }

                foreach (Character character in battleCharacters[currOpponent])
                {
                    board.ReturnCharacter(character);
                    playersInfo[currOpponent].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetStartPower()));
                }
            }
            else
            {
                // Returning all the winner characters to his hand
                foreach (Character character in battleCharacters[winner])
                {
                    board.ReturnCharacter(character);
                    playersInfo[winner].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetStartPower()));
                }

                // Destroying all the loser characters
                Player looser = winner == currPlayer ? currOpponent : currPlayer;
                foreach (Character character in battleCharacters[looser])
                {
                    board.DestroyCharacter(character);
                }
            }

            // Closing the cell
            board.RemoveCell(cell);

            // Stopping the battle
            StopBattle();
        }

        private void StopBattle()
        {
            isBattleNow = false;

            battles[currPlayer].Clear();
            battles[currOpponent].Clear();

            battleCharacters[currPlayer].Clear();
            battleCharacters[currOpponent].Clear();

            battleStatuses[currPlayer] = BattleStatus.NO_BATTLE;
            battleStatuses[currOpponent] = BattleStatus.NO_BATTLE;
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
                    if (battleStatuses[currPlayer] == BattleStatus.NO_CARDS_PLAYED)
                    {
                        battleStatuses[currPlayer] = BattleStatus.ONE_CARD_PLAYED;
                    }
                    else if (battleStatuses[currPlayer] == BattleStatus.ONE_CARD_PLAYED)
                    {
                        battleStatuses[currPlayer] = BattleStatus.TWO_CARDS_PLAYED;
                    }
                }
                else if (card.GetCardType() == CardType.GOLD)
                {
                    battleStatuses[currPlayer] = BattleStatus.TWO_CARDS_PLAYED;
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

        public bool IsBattleNow()
        {
            return isBattleNow;
        }

        public void FinishMove()
        {
            ChangeMove();
        }

        public void MoveCharacter(Character character, Cell cell)
        {
            Cell oldCell = board.GetCharacterCell(character);

            if (oldCell.GetState() != CellState.CLOSED)
            {
                // Removing old effect
                RemoveCellEffectFromCharacter(character, oldCell.GetEffect());

                // Checking if character is alive
                if (board.GetCharacterCell(character) == null) return;

                // Stopping the battle if it was
                if (oldCell.GetState() == CellState.BATTLED)
                {
                    oldCell.SetState(CellState.OPENED);
                    StopBattle();
                }
            }

            // Moving the character
            board.MoveCharacterToCell(character, cell);

            // Checking the new cell
            if (cell.GetState() == CellState.OPENED ||
                cell.GetState() == CellState.BATTLED)
            {
                AddCellEffectToCharacter(character, cell.GetEffect());
            }

            CheckBattlesWithNewCharacter(character, cell);
        }

        public void OpenCell(Cell cell, CellState newState)
        {
            cell.SetState(newState);

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Adding the effect to characters
            foreach (Character character in characters)
            {
                AddCellEffectToCharacter(character, cell.GetEffect());
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

        public void ChangePlayersAfterMoveFinished(bool change)
        {
            changePlayersAfterMoveFinished = change;
        }

        public Player GetCurrMovingPlayer()
        {
            return currPlayer;
        }

        public CardType GetAllowedCardTypes()
        {
            if (isBattleNow)
            {
                BattleStatus battleStatus = battleStatuses[currPlayer];

                if (battleStatus == BattleStatus.NO_CARDS_PLAYED)
                {
                    return CardType.GOLD;
                }
                else if (battleStatus == BattleStatus.ONE_CARD_PLAYED)
                {
                    return CardType.SILVER;
                }
                else
                {
                    return CardType.NEUTRAL;
                }
            }
            else
            {
                return CardType.NEUTRAL;
            }
        }

        public bool AreCharactersAllowed()
        {
            return !isBattleNow;
        }
    }
}
