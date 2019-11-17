using System.Collections.Generic;
using MetaInfo;
using UnityEngine;

namespace BoardStuff
{
    public class BoardController : MonoBehaviour, Board
    {
        private Dictionary<Cell, List<Character>> cells;

        public CellManager cellManager;

        const int CELLS_QUAN = 4;

        // Start is called before the first frame update
        void Start()
        {
            // Creating all the cell prefabs
            cellManager.SetStartPosition(CELLS_QUAN);

            // Creating cell objects
            cells = new Dictionary<Cell, List<Character>>();

            for (int i = 0; i < CELLS_QUAN; i++)
            {
                Cell cell = new CellImpl(i, cellManager);
                cells.Add(cell, new List<Character>());
            }
        }

        public void DestroyCharacter(Character character)
        {
            throw new System.NotImplementedException();
        }

        public void FinishBattle(Cell cell, Player winner)
        {
            throw new System.NotImplementedException();
        }

        public List<Cell> GetAllCells()
        {
            return new List<Cell>(cells.Keys);
        }

        public List<Character> GetAllCharacters()
        {
            List<Character> result = new List<Character>();

            foreach (var pair in cells)
            {
                result.AddRange(pair.Value);
            }

            return result;
        }

        public Cell GetCellById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Cell GetCharacterCell(Character character)
        {
            foreach (var pair in cells)
            {
                if (pair.Value.Contains(character))
                {
                    return pair.Key;
                }
            }

            return null;
        }

        public List<Character> GetCharactersOnCell(Cell cell)
        {
            return cells[cell];
        }

        public void MoveCharacterToCell(Character character, Cell cell)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveCell(Cell cell)
        {
            throw new System.NotImplementedException();
        }

        public void ReturnCharacter(Character character)
        {
            throw new System.NotImplementedException();
        }

        public void SpawnCharacter(Character character, Cell cell)
        {
            throw new System.NotImplementedException();
        }

        public void StartBattle(Cell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}

