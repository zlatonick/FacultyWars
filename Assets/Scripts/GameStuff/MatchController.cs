using System;
using System.Collections.Generic;
using MetaInfo;
using BoardStuff;

namespace GameStuff
{
    public interface MatchController
    {
        List<Character> GetAllCharacters();

        List<Character> GetCharactersOnCell(Cell cell);     // In correct order

        Cell GetCellById(int id);

        Cell GetCharacterCell(Character character);

        List<Cell> GetAllCells();

        Player GetCurrMovingPlayer();

        CardType GetAllowedCardTypes();

        bool AreCharactersAllowed();

        bool IsBattleNow();

        void DontCloseCellAfterBattle(bool dontClose);

        void FinishMove();

        void PlayCard(Card card);

        void PlaceCheck(Check check, Cell cell);

        void ChangePowerSafe(Character character, int changeBy);

        void SetAfterBattleAction(Action<Player> action);

        void SetAfterNTurnsAction(int n, Action action);

        void SetActionAfterCardIsPlayed(Player player, Action<Card> action);

        void FinishBattle(Player winner);

        void MoveCharacter(Character character, Cell cell);

        void ChangePlayersAfterMoveFinished(bool change);

        void ChangeCellEffect(Cell cell, CellEffect effect);

        void OpenCell(Cell cell, CellState newState);

        PlayerInfo GetPlayerInfo(Player player);
    }
}
