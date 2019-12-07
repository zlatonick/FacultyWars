using BoardStuff;
using GameStuff;
using MetaInfo;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        int turnsWithoutPlayingCards;       // 2 turns - finishing the battle
        bool cardPlayedThisTurn;

        Dictionary<Player, List<Battle>> battles;

        Dictionary<Player, List<Character>> battleCharacters;

        Dictionary<Player, BattleStatus> battleStatuses;

        Action<Player> afterBattleAction;

        Dictionary<Action, int> afterNTurnsActions;     // Action -> moves left to action

        // Additional objects

        CheckFactory checkFactory;

        Chooser chooser;

        public MatchControllerImpl(Board board, Player player1, PlayerInfo playerInfo1,
            Player player2, PlayerInfo playerInfo2)
        {
            this.board = board;

            // Setting the players info
            playersInfo = new Dictionary<Player, PlayerInfo>();

            playersInfo.Add(player1, playerInfo1);
            playersInfo.Add(player2, playerInfo2);

            // Choosing the first player    DEBUG
            currPlayer = player1;
            currOpponent = player2;
            /*
            Random random = new Random();

            if (random.Next(0, 2) == 0)
            {
                currPlayer = player1;
                currOpponent = player2;
            }
            else
            {
                currPlayer = player2;
                currOpponent = player1;
            }*/

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
            turnsWithoutPlayingCards = 0;
            cardPlayedThisTurn = false;

            battleStatuses = new Dictionary<Player, BattleStatus>();

            battleStatuses.Add(player1, BattleStatus.NO_BATTLE);
            battleStatuses.Add(player2, BattleStatus.NO_BATTLE);

            afterBattleAction = null;
            afterNTurnsActions = new Dictionary<Action, int>();
        }

        private static int GetRandomEffectPower(System.Random random)
        {
            if (random.Next(2) == 0)
            {
                return random.Next(1, 3) * 10;
            }
            else
            {
                return -random.Next(1, 3) * 10;
            }
        }

        private static CellEffect GetRandomCellEffect()
        {
            /*
             * Generating a random cell effect
             * 50% - cell has two effects
             * 40% - cell has one effect
             * 10% - cell has no effects
             * Every effect and its value is chosen randomly with equal probabilities
             */

            System.Random random = new System.Random();

            int effectsQuan = random.Next(10);

            if (effectsQuan < 5)
            {
                int stuffInt = random.Next(3);

                int[] numbers = new int[2];
                int index = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i != stuffInt)
                    {
                        numbers[index] = i;
                        index++;
                    }
                }

                int stuffInt2 = numbers[random.Next(2)];

                return new CellEffect((StuffClass)stuffInt, GetRandomEffectPower(random),
                    (StuffClass)stuffInt2, GetRandomEffectPower(random));
            }
            else if (effectsQuan < 9)
            {
                return new CellEffect((StuffClass)random.Next(3), GetRandomEffectPower(random));
            }
            else
            {
                return new CellEffect();
            }
        }

        private void ChangeMove()
        {
            // Tracking the battle status
            if (isBattleNow && !cardPlayedThisTurn)
            {
                turnsWithoutPlayingCards++;

                if (turnsWithoutPlayingCards >= 2)
                {
                    turnsWithoutPlayingCards = 0;
                    FinishBattle(DetermineCurrBattleWinner());
                }
            }
            else
            {
                turnsWithoutPlayingCards = 0;
            }
            cardPlayedThisTurn = false;

            // Swapping the players
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

            // Executing the action, if needed
            if (afterNTurnsActions.Count > 0)
            {
                var keys = afterNTurnsActions.Keys;
                foreach (var key in keys)
                {
                    afterNTurnsActions[key]--;

                    if (afterNTurnsActions[key] <= 0)
                    {
                        key();
                        afterNTurnsActions.Remove(key);
                    }
                }
            }
        }

        public void ChangePowerSafe(Character character, int changeBy)
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
            Character newCharacter = board.SpawnCharacter(check.GetStuffClass(),
                check.GetLevel(), check.GetPower(), currPlayer, cell);

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
                if (characters.Count - 1 > enemies.Count && isBattleNow)
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
                    turnsWithoutPlayingCards = -1;      // First move is not considered

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

        public PlayerInfo GetPlayerInfo(Player player)
        {
            return playersInfo[player];
        }

        public List<Character> GetCharactersOnCell(Cell cell)
        {
            return board.GetCharactersOnCell(cell);
        }

        public void ChangeCellEffect(Cell cell, CellEffect effect)
        {
            // Redrawing the cell
            CellEffect oldEffect = cell.GetEffect();

            board.RemoveCellEffect(cell);
            board.SetCellEffect(cell, effect);

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

        private Player DetermineCurrBattleWinner()
        {
            int totalPower = 0;
            int totalOpponentPower = 0;

            foreach (Character character in battleCharacters[currPlayer])
            {
                totalPower += character.GetPower();
            }
            foreach (Character character in battleCharacters[currOpponent])
            {
                totalOpponentPower += character.GetPower();
            }

            if (totalPower > totalOpponentPower)
            {
                return currPlayer;
            }
            if (totalPower < totalOpponentPower)
            {
                return currOpponent;
            }

            return null;        // Draw
        }

        public void FinishBattle(Player winner)
        {
            if (winner != null)
            {
                Debug.Log("The battle is finished. The winner is player " + winner.id);
            }
            else
            {
                Debug.Log("The battle is finished. Draw");
            }

            Cell cell = battles[currPlayer][0].GetCell();

            // Drawing the battle finishing
            board.FinishBattle(cell, winner);

            // Removing the characters
            if (winner == null)
            {
                // Draw. Returning all the characters to hands
                foreach (Character character in battleCharacters[currPlayer])
                {
                    board.DestroyCharacter(character);
                    playersInfo[currPlayer].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetLevel()));
                }

                foreach (Character character in battleCharacters[currOpponent])
                {
                    board.DestroyCharacter(character);
                    playersInfo[currOpponent].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetLevel()));
                }
            }
            else
            {
                // Returning all the winner characters to his hand
                foreach (Character character in battleCharacters[winner])
                {
                    board.DestroyCharacter(character);
                    playersInfo[winner].AddCheckToHand(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetLevel()));
                }

                // Destroying all the loser characters
                Player looser = winner == currPlayer ? currOpponent : currPlayer;
                foreach (Character character in battleCharacters[looser])
                {
                    board.DestroyCharacter(character);
                    playersInfo[looser].AddCheckToDead(checkFactory.GetCheck(
                        character.GetStuffClass(), character.GetLevel()));
                }
            }

            // Closing the cell
            board.RemoveCell(cell);

            // Stopping the battle
            StopBattle();

            // Executing after battle action
            if (afterBattleAction != null)
            {
                afterBattleAction(winner);
                afterBattleAction = null;
            }
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
            cardPlayedThisTurn = true;
            playersInfo[currPlayer].AddCardToPlayed(card);

            if (card.IsChoosing())
            {
                card.Choose(chooser);
            }

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

        public Cell GetCellById(int id)
        {
            return board.GetCellById(id);
        }

        public Cell GetCharacterCell(Character character)
        {
            return board.GetCharacterCell(character);
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
            CellEffect effect = GetRandomCellEffect();

            cell.SetState(newState);
            cell.SetEffect(effect);

            board.OpenCell(cell);
            board.SetCellEffect(cell, effect);

            List<Character> characters = board.GetCharactersOnCell(cell);

            // Adding the effect to characters
            foreach (Character character in characters)
            {
                AddCellEffectToCharacter(character, cell.GetEffect());
            }
        }

        public void SetAfterBattleAction(Action<Player> action)
        {
            if (afterBattleAction == null)
            {
                afterBattleAction = action;
            }
            else
            {
                afterBattleAction += action;
            }
        }

        public void SetAfterNTurnsAction(int n, Action action)
        {
            afterNTurnsActions.Add(action, n * 2);
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
                return CardType.NO_BATTLE;
            }
        }

        public bool AreCharactersAllowed()
        {
            return !isBattleNow;
        }
    }
}
