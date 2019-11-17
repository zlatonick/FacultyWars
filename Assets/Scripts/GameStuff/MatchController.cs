using System;
using System.Collections.Generic;
using MetaInfo;
using BoardStuff;

namespace GameStuff
{
    public interface MatchController
    {
        List<Character> GetAllCharacters();

        List<Cell> GetAllCells();

        PlayerInfo GetPlayerInfo(Player player);

        bool IsBattleNow();

        void PlaceCheck(Check check, Cell cell);

        void SetAfterBattleAction(Action<Player> action);

        void SetAfterNTurnsAction(Action action, int n);

        void FinishBattle(Player winner);

        void MoveCharacter(Character character, Cell cell);

        void SetNextTurnPlayer(Player player);

        void ChangeCellEffect(Cell cell, CellEffect effect);

        void OpenCell(Cell cell);
    }
}
