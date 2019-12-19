using UnityEngine;
using System.Collections.Generic;
using MetaInfo;
using UnityEngine.UI;
using System;
using System.Collections;

namespace BoardStuff
{
    public class BoardStuffManager : MonoBehaviour
    {
        public Canvas canvas;

        // Additional classes to store cell and character info
        class CellEffect
        {
            public int effectsQuan;

            public GameObject icon;
            public GameObject icon_2;

            public CellEffect()
            {
                effectsQuan = 0;
            }

            public CellEffect(GameObject icon)
            {
                effectsQuan = 1;
                this.icon = icon;
            }

            public CellEffect(GameObject icon, GameObject icon_2)
            {
                effectsQuan = 2;
                this.icon = icon;
                this.icon_2 = icon_2;
            }
        }

        class CharacterInfo
        {
            public int id;

            public bool isBottom;

            public CharacterClickHandler charPrefab;

            public CharacterInfo(int id, bool isBottom, CharacterClickHandler charPrefab)
            {
                this.id = id;
                this.isBottom = isBottom;
                this.charPrefab = charPrefab;
            }
        }

        class CellCharacters
        {
            public List<CharacterInfo> upper;

            public List<CharacterInfo> lower;

            public CellCharacters()
            {
                upper = new List<CharacterInfo>();
                lower = new List<CharacterInfo>();
            }

            public void AddCharacter(CharacterInfo character, bool toLower)
            {
                if (toLower) lower.Add(character);
                else upper.Add(character);
            }

            // Returns true if was lower
            public bool RemoveCharacter(CharacterInfo character)
            {
                if (upper.Contains(character))
                {
                    upper.Remove(character);
                    return false;
                }
                else
                {
                    lower.Remove(character);
                    return true;
                }
            }
        }

        // ---------------- Cells

        public CellClickHandler cellPrefab;

        // Faculty icons game objects
        public GameObject iasaIcon;

        public GameObject fictIcon;

        public GameObject fpmIcon;

        // Cell colors
        private Color closedColor;
        private Color openedColor;
        private Color closedHighlightedColor;
        private Color openedHighlightedColor;

        // Created cells
        private Dictionary<int, CellClickHandler> cells;

        // Cell effects
        private Dictionary<int, CellEffect> cellEffects;

        // Cell states
        private Dictionary<int, CellState> cellStates;

        // Numeric cell data
        private float cellHeight;
        private float cellWidth;
        private float cellWidthClear;
        private float cellOffset;

        // Actions
        private Action<int> cellClickedAction;

        private Action<int, int> characterClickedAction;        // Takes characterId and cellID

        // ---------------- Characters

        // Character game objects
        public CharacterClickHandler characterRed;

        public CharacterClickHandler characterBlue;

        public CharacterClickHandler characterGreen;

        public GameObject changePowerText;

        private float characterWidth;
        private float characterOffset;

        // Character game objects (holded in structure)
        private Dictionary<StuffClass, CharacterClickHandler> charGameObjects;

        // Characters on cells
        private Dictionary<int, CellCharacters> characters;

        // Inforamtion about characters' cells (charId -> cellId)
        private Dictionary<int, int> charsCells;

        void Start()
        {
            // Cells
            var cellPrefabRect = cellPrefab.gameObject.GetComponent<RectTransform>();
            cellWidth = cellPrefabRect.sizeDelta.x * cellPrefabRect.localScale.x;
            cellHeight = cellPrefabRect.sizeDelta.y * cellPrefabRect.localScale.y;

            cellOffset = 44;
            cellWidthClear = cellWidth - cellOffset;

            cells = new Dictionary<int, CellClickHandler>();
            cellEffects = new Dictionary<int, CellEffect>();
            cellStates = new Dictionary<int, CellState>();

            closedColor = cellPrefab.GetComponent<Image>().color;
            closedHighlightedColor = new Color(closedColor.r,
                closedColor.g, closedColor.b, 0.78f);

            openedColor = new Color(0.91f, 0.71f, 0.255f);
            openedHighlightedColor = new Color(0.91f, 0.71f, 0.255f, 0.78f);

            // Characters
            charGameObjects = new Dictionary<StuffClass, CharacterClickHandler>();

            charGameObjects.Add(StuffClass.IASA, characterRed);
            charGameObjects.Add(StuffClass.FICT, characterGreen);
            charGameObjects.Add(StuffClass.FPM, characterBlue);

            characters = new Dictionary<int, CellCharacters>();
            charsCells = new Dictionary<int, int>();

            var characterRect = characterRed.gameObject.GetComponent<RectTransform>();
            characterWidth = characterRect.sizeDelta.x * characterRect.localScale.x;
            characterOffset = 23;
        }

        public void SetCellClickedAction(Action<int> cellClickedAction)
        {
            this.cellClickedAction = cellClickedAction;
        }

        public void SetCharacterClickedAction(Action<int, int> characterClickedAction)
        {
            this.characterClickedAction = characterClickedAction;
        }

        public void FillBoardWithCells(int pairCellsQuan)
        {
            for (int i = 0; i < pairCellsQuan; i++)
            {
                // Bottom cell
                Vector2 bottomCellPos = new Vector2(
                    -cellWidth / 2 + cellWidthClear * (i - 1),
                    -cellHeight / 2);

                CellClickHandler bottomCell = Instantiate(cellPrefab, transform, false);
                bottomCell.transform.localPosition = bottomCellPos;

                bottomCell.SetCanvas(canvas);
                bottomCell.SetClickAction(CellClicked);

                cells.Add(2 * i + 1, bottomCell);
                cellStates.Add(2 * i + 1, CellState.CLOSED);

                // Top cell
                Vector2 topCellPos = new Vector2(bottomCellPos.x + cellOffset + 2, cellHeight / 2);

                CellClickHandler topCell = Instantiate(cellPrefab, transform, false);
                topCell.transform.localPosition = topCellPos;

                topCell.SetCanvas(canvas);
                topCell.SetClickAction(CellClicked);

                cells.Add(2 * i, topCell);
                cellStates.Add(2 * i, CellState.CLOSED);
            }
        }

        private GameObject GetIconOfStuffClass(StuffClass stuffClass)
        {
            switch (stuffClass)
            {
                case StuffClass.IASA:
                    return iasaIcon;

                case StuffClass.FICT:
                    return fictIcon;

                case StuffClass.FPM:
                    return fpmIcon;
            }
            return null;
        }

        public void HighlightPlacableCells(List<int> placableCells)
        {
            foreach (var pr in cells)
            {
                if (placableCells.Contains(pr.Key))
                {
                    Image image = pr.Value.GetComponent<Image>();

                    if (cellStates[pr.Key] == CellState.CLOSED)
                        image.color = closedHighlightedColor;
                    else
                        image.color = openedHighlightedColor;
                }
            }
        }

        public void UnhighlightCells()
        {
            foreach (var pr in cells)
            {
                Image image = pr.Value.GetComponent<Image>();

                if (cellStates[pr.Key] == CellState.CLOSED)
                    image.color = closedColor;
                else
                    image.color = openedColor;
            }
        }

        public void OpenCell(int cellId)
        {
            cellStates[cellId] = CellState.OPENED;

            Animator animator = cells[cellId].GetComponent<Animator>();
            animator.SetInteger("cellState", 1);
        }

        public void SetEffect(int cellId, StuffClass stuffClass, int power)
        {
            CellClickHandler cell = cells[cellId];
            Vector2 cellPos = cell.transform.localPosition;

            GameObject stuffClassIcon = Instantiate(GetIconOfStuffClass(stuffClass),
                cell.transform, false);

            stuffClassIcon.transform.localPosition = new Vector2(
                -0.177f * cellWidth, 0);

            Text middleText = cell.transform.Find("text_middle").gameObject.GetComponent<Text>();
            middleText.text = "" + (power > 0 ? "+" : "") + power;

            cellEffects.Add(cellId, new CellEffect(stuffClassIcon));
        }

        public void SetEffect(int cellId, StuffClass stuffClass, int power,
            StuffClass stuffClass2, int power2)
        {
            CellClickHandler cell = cells[cellId];
            Vector2 cellPos = cell.transform.localPosition;

            GameObject stuffClassIcon = Instantiate(GetIconOfStuffClass(stuffClass),
                cell.transform, false);
            GameObject stuffClassIcon2 = Instantiate(GetIconOfStuffClass(stuffClass2),
                cell.transform, false);

            stuffClassIcon.transform.localPosition = new Vector2(
                -0.13f * cellWidth, cellHeight / 4);
            stuffClassIcon2.transform.localPosition = new Vector2(
                -0.224f * cellWidth, -cellHeight / 4);

            Text topText = cell.transform.Find("text_top").gameObject.GetComponent<Text>();
            Text bottomText = cell.transform.Find("text_bottom").gameObject.GetComponent<Text>();

            topText.text = "" + (power > 0 ? "+" : "") + power;
            bottomText.text = "" + (power2 > 0 ? "+" : "") + power2;

            cellEffects.Add(cellId, new CellEffect(stuffClassIcon, stuffClassIcon2));
        }

        public void RemoveEffect(int cellId)
        {
            if (!cellEffects.ContainsKey(cellId)) return;

            GameObject cell = cells[cellId].gameObject;
            CellEffect effect = cellEffects[cellId];

            if (effect.effectsQuan == 1)
            {
                Destroy(effect.icon);

                Text middleText = cell.transform.Find("text_middle").gameObject.GetComponent<Text>();
                middleText.text = "";
            }
            else if (effect.effectsQuan == 2)
            {
                Destroy(effect.icon);
                Destroy(effect.icon_2);

                Text topText = cell.transform.Find("text_top").gameObject.GetComponent<Text>();
                Text bottomText = cell.transform.Find("text_bottom").gameObject.GetComponent<Text>();

                topText.text = "";
                bottomText.text = "";
            }

            cellEffects.Remove(cellId);
        }

        public void RemoveCell(int cellId)
        {
            RemoveEffect(cellId);

            // Animating the vanish
            Animator animator = cells[cellId].GetComponent<Animator>();
            animator.SetInteger("cellState", 2);

            StartCoroutine(DestoryCellAfterFewSeconds(cells[cellId].gameObject));

            cells.Remove(cellId);
            cellEffects.Remove(cellId);
            cellStates.Remove(cellId);
        }

        private IEnumerator DestoryCellAfterFewSeconds(GameObject cell)
        {
            yield return new WaitForSeconds(2);
            Destroy(cell);
        }

        private bool PointIsInCell(Vector2 vecX, Vector2 vecY, Vector2 point)
        {
            float det = vecX.x * vecY.y - vecX.y * vecY.x;
            float coordX = (vecY.y * point.x - vecY.x * point.y) / det;
            float coordY = (vecX.y * point.x + vecX.x * point.y) / det;

            return coordX >= 0 && coordX <= 1 && coordY >= 0 && coordY <= 1;
        }

        public int GetCellIdByCoords(Vector2 coords)
        {
            Vector2 vecX = new Vector2(cellWidthClear, 0);
            Vector2 vecY = new Vector2(cellOffset, cellHeight);

            foreach (var pr in cells)
            {
                Vector2 pointVec = new Vector2(
                    coords.x - pr.Value.transform.localPosition.x + (cellWidth / 2),
                    coords.y - pr.Value.transform.localPosition.y + (cellHeight / 2));

                if (PointIsInCell(vecX, vecY, pointVec))
                {
                    return pr.Key;
                }
            }
            return -1;
        }

        private CharacterInfo FindCharacter(int characterId)
        {
            CellCharacters cellCharacters = characters[charsCells[characterId]];

            foreach (CharacterInfo charInfo in cellCharacters.upper)
            {
                if (charInfo.id == characterId)
                {
                    return charInfo;
                }
            }
            foreach (CharacterInfo charInfo in cellCharacters.lower)
            {
                if (charInfo.id == characterId)
                {
                    return charInfo;
                }
            }

            return null;
        }

        // From left to right
        private List<Vector2> GetCharactersCoords(int cellId, bool toBottom, int charsQuan)
        {
            List<Vector2> result = new List<Vector2>();

            Vector2 cellCoords = cells[cellId].transform.localPosition;

            float charWidthClear = characterWidth - characterOffset;
            float initialPosX = cellCoords.x - (cellWidth / 2)
                + (charWidthClear / 2) - (characterOffset / 2);
            float charCoordY = cellCoords.y;

            if (toBottom)
            {
                initialPosX += cellOffset / 4;
                charCoordY -= cellHeight / 4;
            }
            else
            {
                initialPosX += cellOffset * 3 / 4;
                charCoordY += cellHeight / 4;
            }

            if (charsQuan == 1)
            {
                Vector2 charCoords;
                if (toBottom)
                {
                    charCoords = new Vector2(cellCoords.x - cellOffset / 4 - characterOffset / 2,
                        cellCoords.y - cellHeight / 4);
                }
                else
                {
                    charCoords = new Vector2(cellCoords.x + cellOffset / 4 - characterOffset / 2,
                        cellCoords.y + cellHeight / 4);
                }

                result.Add(charCoords);
                /*float charsDist = (cellWidthClear - totalCharsWidth) / (charsQuan + 1);
                initialPosX += charsDist;

                for (int i = 0; i < charsQuan; i++)
                {
                    float charCoordX = initialPosX + i * (charsDist + characterWidth - characterOffset);
                    Debug.Log("charCoordX: " + charCoordX);
                    result.Add(new Vector2(charCoordX, charCoordY));
                }*/
            }
            else
            {
                float margin = cellWidthClear / 10;

                float charsDist = (cellWidthClear - 2 * margin - charWidthClear) / (charsQuan - 1);
                initialPosX += margin;

                for (int i = 0; i < charsQuan; i++)
                {
                    float charCoordX = initialPosX + i * charsDist;
                    result.Add(new Vector2(charCoordX, charCoordY));
                }
            }

            return result;
        }

        private void NormalizeCharacterPositions(int cellId, bool isBottom)
        {
            List<CharacterInfo> charactersLine = isBottom ?
                characters[cellId].lower : characters[cellId].upper;

            if (charactersLine.Count > 0)
            {
                List<Vector2> charsCoords = GetCharactersCoords(cellId, isBottom, charactersLine.Count);

                for (int i = 0; i < charactersLine.Count; i++)
                {
                    CharacterClickHandler charPrefab = charactersLine[i].charPrefab;
                    charPrefab.transform.localPosition = charsCoords[i];
                }
            }
        }

        public void SpawnCharacter(int cellId, int characterId, StuffClass stuffClass,
            int power, bool toBottom)
        {
            // Spawning the character
            CharacterClickHandler newCharPrefab = Instantiate(
                charGameObjects[stuffClass], transform, false);
            newCharPrefab.SetCanvas(canvas);
            newCharPrefab.SetClickAction(CharacterClicked);

            // Quantity of characters in the line
            if (!characters.ContainsKey(cellId))
            {
                characters.Add(cellId, new CellCharacters());
            }

            // Setting the power
            Text powerText = newCharPrefab.GetComponentInChildren<Text>();
            powerText.text = "" + power;

            CharacterInfo charInfo = new CharacterInfo(characterId, toBottom, newCharPrefab);

            // Adding character to the board info
            characters[cellId].AddCharacter(charInfo, toBottom);
            charsCells.Add(characterId, cellId);

            // Normalizing the positions
            NormalizeCharacterPositions(cellId, toBottom);
        }

        public void RemoveCharacter(int characterId)
        {
            CharacterInfo character = FindCharacter(characterId);

            // Destroying the character
            Destroy(character.charPrefab.gameObject);

            // Removing character from structures
            int cellId = charsCells[characterId];
            bool wasBottom = characters[cellId].RemoveCharacter(character);
            charsCells.Remove(characterId);

            // Recalculating the positions of the rest
            NormalizeCharacterPositions(cellId, wasBottom);
        }

        public void MoveCharacterToCell(int characterId, int newCellId)
        {
            CharacterInfo character = FindCharacter(characterId);
            int oldCellId = charsCells[characterId];

            // Removing from the old cell
            bool wasBottom = characters[oldCellId].RemoveCharacter(character);
            NormalizeCharacterPositions(oldCellId, wasBottom);

            // Adding to the new cell
            characters[newCellId].AddCharacter(character, wasBottom);
            NormalizeCharacterPositions(newCellId, wasBottom);

            charsCells[characterId] = newCellId;
        }

        public void ChangeCharacterPower(int characterId, int newPower, int changeBy)
        {
            CharacterInfo character = FindCharacter(characterId);
            Text powerText = character.charPrefab.GetComponentInChildren<Text>();
            powerText.text = "" + newPower;

            // Playing the animation
            if (changeBy != 0)
            {
                GameObject powerTextInst = Instantiate(changePowerText,
                    character.charPrefab.transform, false);
                powerTextInst.transform.localPosition = new Vector2(0, 0);
                powerTextInst.transform.localScale = new Vector2(4, 4);

                Text text = powerTextInst.GetComponent<Text>();
                text.text = changeBy > 0 ? "+" : "";
                text.text += changeBy;

                Animator animator = powerTextInst.GetComponent<Animator>();
                animator.SetInteger("PowerChanged", 1);

                StartCoroutine(DeletePowerText(powerTextInst));
            }
        }

        private IEnumerator DeletePowerText(GameObject powerText)
        {
            yield return new WaitForSeconds(3);
            Destroy(powerText);
        }

        public void CellClicked(Vector2 pos)
        {
            int cellId = GetCellIdByCoords(pos);
            cellClickedAction(cellId);
        }

        public void CharacterClicked(CharacterClickHandler characterClickHandler, Vector2 pos)
        {
            int cellId = GetCellIdByCoords(pos);

            foreach (CharacterInfo info in characters[cellId].upper)
            {
                if (info.charPrefab == characterClickHandler)
                {
                    characterClickedAction(info.id, cellId);
                    return;
                }
            }
            foreach (CharacterInfo info in characters[cellId].lower)
            {
                if (info.charPrefab == characterClickHandler)
                {
                    characterClickedAction(info.id, cellId);
                    return;
                }
            }
        }
    }
}