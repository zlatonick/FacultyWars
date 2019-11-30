using System.Collections.Generic;
using MetaInfo;
using UnityEngine;

namespace BoardStuff
{
    public interface Board
    {
        List<Character> GetAllCharacters();

        List<Cell> GetAllCells();

        Character SpawnCharacter(StuffClass stuffClass, int level, int power, Player player, Cell cell);

        void DestroyCharacter(Character character);

        void StartBattle(Cell cell);

        void FinishBattle(Cell cell, Player winner);

        List<Character> GetCharactersOnCell(Cell cell);

        Cell GetCharacterCell(Character character);

        void MoveCharacterToCell(Character character, Cell cell);

        void RemoveCell(Cell cell);

        void OpenCell(Cell cell);

        Cell GetCellById(int id);

        Cell GetCellByCoords(Vector2 coords);

        void SetCellEffect(Cell cell, CellEffect cellEffect);

        void RemoveCellEffect(Cell cell);
    }
}
