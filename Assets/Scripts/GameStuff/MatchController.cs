using System;
using System.Collections.Generic;
using MetaInfo;
using BoardStuff;

namespace GameStuff
{
    public interface MatchController
    {
        List<Character> GetAllCharacters();

        Cell GetCellById(int id);

        List<Cell> GetAllCells();

        Player GetCurrMovingPlayer();

        CardType GetAllowedCardTypes();

        bool AreCharactersAllowed();

        bool IsBattleNow();

        void FinishMove();

        void PlayCard(Card card);

        void PlaceCheck(Check check, Cell cell);

        void SetAfterBattleAction(Action<Player> action);

        void SetAfterNTurnsAction(Action action, int n);

        void FinishBattle(Player winner);

        void MoveCharacter(Character character, Cell cell);

        void ChangePlayersAfterMoveFinished(bool change);

        void ChangeCellEffect(Cell cell, CellEffect effect);

        void OpenCell(Cell cell, CellState newState);
    }
}
