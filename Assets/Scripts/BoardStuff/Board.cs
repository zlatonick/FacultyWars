using System;
using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public interface Board
    {
        List<Character> GetAllCharacters();

        List<Cell> GetAllCells();

        Cell GetCellById(int id);

        void SpawnCharacter(Character character, Cell cell);

        void DestroyCharacter(Character character);

        void ReturnCharacter(Character character);

        void StartBattle(Cell cell);

        void FinishBattle(Cell cell, Player winner);

        List<Character> GetCharactersOnCell(Cell cell);

        Cell GetCharacterCell(Character character);

        void MoveCharacterToCell(Character character, Cell cell);

        void RemoveCell(Cell cell);
    }
}
