using System.Collections.Generic;
using MetaInfo;
using UnityEngine;

namespace BoardStuff
{
    public class BoardController : Board
    {
        private BoardStuffManager boardStuffManager;

        private Dictionary<Cell, List<Character>> cells;

        private CharacterFactory characterFactory;

        public BoardController(BoardStuffManager boardStuffManager, int cellsPairsQuan)
        {
            this.boardStuffManager = boardStuffManager;

            // Creating all the cell prefabs
            boardStuffManager.FillBoardWithCells(cellsPairsQuan);

            // Creating cell objects
            cells = new Dictionary<Cell, List<Character>>();

            for (int i = 0; i < 2 * cellsPairsQuan; i++)
            {
                Cell cell = new CellImpl(i, boardStuffManager);
                cells.Add(cell, new List<Character>());
            }

            // Creating character factory
            characterFactory = new CharacterFactory(boardStuffManager);
        }

        public void DestroyCharacter(Character character)
        {
            boardStuffManager.RemoveCharacter(character.GetId());
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
            boardStuffManager.RemoveCell(cell.GetId());
        }

        public void ReturnCharacter(Character character)
        {
            throw new System.NotImplementedException();
        }

        public Character SpawnCharacter(StuffClass stuffClass, int power, Player player, Cell cell)
        {
            Character character = characterFactory.CreateCharacter(stuffClass, power, player);

            boardStuffManager.SpawnCharacter(cell.GetId(), character.GetId(),
                stuffClass, power, player.id == 0);     // Main player has id = 0

            return character;
        }

        public void StartBattle(Cell cell)
        {
            throw new System.NotImplementedException();
        }

        public Cell GetCellByCoords(Vector2 coords)
        {
            throw new System.NotImplementedException();
        }
    }
}

