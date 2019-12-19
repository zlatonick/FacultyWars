using System;
using System.Collections.Generic;
using MetaInfo;
using UnityEngine;

namespace BoardStuff
{
    public class BoardController : Board
    {
        private BoardStuffManager boardStuffManager;

        private int cellsPairsQuan;

        private Dictionary<int, Cell> cells;

        private Dictionary<Cell, List<Character>> cellsCharacters;

        private CharacterFactory characterFactory;

        private Action<Cell> cellClickedAction;

        private Action<Character> characterClickedAction;

        public BoardController(BoardStuffManager boardStuffManager, int cellsPairsQuan,
            Action<Cell> cellClickedAction, Action<Character> characterClickedAction)
        {
            this.boardStuffManager = boardStuffManager;
            this.cellsPairsQuan = cellsPairsQuan;

            // Creating cell objects
            cells = new Dictionary<int, Cell>();
            cellsCharacters = new Dictionary<Cell, List<Character>>();

            // Actions
            this.cellClickedAction = cellClickedAction;
            this.characterClickedAction = characterClickedAction;

            this.boardStuffManager.SetCellClickedAction(OnCellClicked);
            this.boardStuffManager.SetCharacterClickedAction(OnCharacterClicked);

            // Creating character factory
            characterFactory = new CharacterFactory(boardStuffManager);

            FillBoard();
        }

        private void FillBoard()
        {
            // Creating all the cell prefabs
            boardStuffManager.FillBoardWithCells(cellsPairsQuan);

            for (int i = 0; i < 2 * cellsPairsQuan; i++)
            {
                Cell cell = new CellImpl(i);
                cells.Add(i, cell);
                cellsCharacters.Add(cell, new List<Character>());
            }
        }

        public void DestroyCharacter(Character character)
        {
            boardStuffManager.RemoveCharacter(character.GetId());
            cellsCharacters[GetCharacterCell(character)].Remove(character);
        }

        public void FinishBattle(Cell cell, Player winner)
        {
            // TODO
        }

        public List<Cell> GetAllCells()
        {
            return new List<Cell>(cells.Values);
        }

        public List<Character> GetAllCharacters()
        {
            List<Character> result = new List<Character>();

            foreach (var pair in cellsCharacters)
            {
                result.AddRange(pair.Value);
            }

            return result;
        }

        public Cell GetCharacterCell(Character character)
        {
            foreach (var pair in cellsCharacters)
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
            return cellsCharacters[cell];
        }

        public void MoveCharacterToCell(Character character, Cell cell)
        {
            cellsCharacters[GetCharacterCell(character)].Remove(character);
            cellsCharacters[cell].Add(character);

            boardStuffManager.MoveCharacterToCell(character.GetId(), cell.GetId());
        }

        public void RemoveCell(Cell cell)
        {
            boardStuffManager.RemoveCell(cell.GetId());

            cells.Remove(cell.GetId());
            cellsCharacters.Remove(cell);

            // Checking if no cells left
            if (cells.Count == 0)
            {
                FillBoard();
            }
        }

        public void HighlightCells(List<Cell> cells)
        {
            List<int> indexes = new List<int>();

            foreach (Cell cell in cells)
            {
                indexes.Add(cell.GetId());
            }

            boardStuffManager.HighlightPlacableCells(indexes);
        }

        public void UnhighlightCells()
        {
            boardStuffManager.UnhighlightCells();
        }

        public Character SpawnCharacter(StuffClass stuffClass, int level, int power, Player player, Cell cell)
        {
            Character character = characterFactory.CreateCharacter(stuffClass, level, power, player);

            boardStuffManager.SpawnCharacter(cell.GetId(), character.GetId(),
                stuffClass, power, player.id == 0);     // Main player has id = 0

            cellsCharacters[cell].Add(character);

            return character;
        }

        public void OpenCell(Cell cell)
        {
            boardStuffManager.OpenCell(cell.GetId());
        }

        public void SetCellEffect(Cell cell, CellEffect cellEffect) 
        {
            if (cellEffect.EffectsQuan == 1)
            {
                boardStuffManager.SetEffect(cell.GetId(), cellEffect.StuffClass, cellEffect.Power);
            }
            else if (cellEffect.EffectsQuan == 2)
            {
                boardStuffManager.SetEffect(cell.GetId(), cellEffect.StuffClass, cellEffect.Power,
                    cellEffect.StuffClass2, cellEffect.Power2);
            }
        }

        public void RemoveCellEffect(Cell cell)
        {
            boardStuffManager.RemoveEffect(cell.GetId());
        }

        public void StartBattle(Cell cell)
        {
            // TODO
        }

        public Cell GetCellByCoords(Vector2 coords)
        {
            int cellId = boardStuffManager.GetCellIdByCoords(coords);

            if (cellId == -1) return null;
            Cell cell = cells[cellId];

            return cell;
        }

        public Cell GetCellById(int id)
        {
            return cells[id];
        }

        public void OnCellClicked(int cellId)
        {
            cellClickedAction(cells[cellId]);
        }

        public void OnCharacterClicked(int characterId, int cellId)
        {
            // Finding the character by id
            foreach (Character character in cellsCharacters[cells[cellId]])
            {
                if (character.GetId() == characterId)
                {
                    characterClickedAction(character);
                    break;
                }
            }
        }
    }
}

