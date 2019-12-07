using UnityEngine;
using System.Collections.Generic;
using MetaInfo;
using UnityEngine.UI;

namespace BoardStuff
{
    public class BoardStuffManager : MonoBehaviour
    {
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

            public GameObject charPrefab;

            public CharacterInfo(int id, bool isBottom, GameObject charPrefab)
            {
                this.id = id;
                this.isBottom = isBottom;
                this.charPrefab = charPrefab;
            }
        }

        // ---------------- Cells

        public GameObject cellPrefab;

        public Sprite cellSprite;

        // Faculty icons game objects
        public GameObject iasaIcon;

        public GameObject fictIcon;

        public GameObject fpmIcon;

        // Created cells
        private Dictionary<int, GameObject> cells;

        // Cell effects
        private Dictionary<int, CellEffect> cellEffects;

        // Numeric cell data
        private float cellHeight;
        private float cellWidth;
        private float cellWidthClear;
        private float cellOffset;

        // ---------------- Characters

        // Character game objects
        public GameObject characterRed;

        public GameObject characterBlue;

        public GameObject characterGreen;

        private float characterOffset;

        // Character game objects (holded in structure)
        private Dictionary<StuffClass, GameObject> charGameObjects;

        // Characters on cells
        private Dictionary<int, List<CharacterInfo>> characters;

        // Inforamtion about characters' cells (charId -> cellId)
        private Dictionary<int, int> charsCells;

        void Start()
        {
            // Cells
            var cellPrefabRect = cellPrefab.GetComponent<RectTransform>();
            cellWidth = cellPrefabRect.sizeDelta.x * cellPrefabRect.localScale.x;
            cellHeight = cellPrefabRect.sizeDelta.y * cellPrefabRect.localScale.y;

            cellOffset = 49;
            cellWidthClear = cellWidth - cellOffset;

            cells = new Dictionary<int, GameObject>();
            cellEffects = new Dictionary<int, CellEffect>();

            // Characters
            charGameObjects = new Dictionary<StuffClass, GameObject>();

            charGameObjects.Add(StuffClass.IASA, characterRed);
            charGameObjects.Add(StuffClass.FICT, characterBlue);
            charGameObjects.Add(StuffClass.FPM, characterGreen);

            characters = new Dictionary<int, List<CharacterInfo>>();
            charsCells = new Dictionary<int, int>();

            characterOffset = 23;
        }

        public void FillBoardWithCells(int pairCellsQuan)
        {
            for (int i = 0; i < pairCellsQuan; i++)
            {
                // Bottom cell
                Vector2 bottomCellPos = new Vector2(
                    -cellWidth / 2 + cellWidthClear * (i - 1),
                    -cellHeight / 2);

                GameObject bottomCell = Instantiate(cellPrefab, transform, false);
                bottomCell.transform.localPosition = bottomCellPos;
                cells.Add(2 * i + 1, bottomCell);

                // Top cell
                Vector2 topCellPos = new Vector2(bottomCellPos.x + cellOffset + 2, cellHeight / 2);

                GameObject topCell = Instantiate(cellPrefab, transform, false);
                topCell.transform.localPosition = topCellPos;
                cells.Add(2 * i, topCell);
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

        public void OpenCell(int cellId)
        {
            Animator animator = cells[cellId].GetComponent<Animator>();
            animator.SetInteger("cellState", 1);
        }

        public void SetEffect(int cellId, StuffClass stuffClass, int power)
        {
            GameObject cell = cells[cellId];
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
            GameObject cell = cells[cellId];
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

            GameObject cell = cells[cellId];
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

            cells.Remove(cellId);
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
            // Finding the character
            CharacterInfo character = null;

            foreach (CharacterInfo charInfo in characters[charsCells[characterId]])
            {
                if (charInfo.id == characterId)
                {
                    character = charInfo;
                    break;
                }
            }

            return character;
        }

        private Vector2 GetCharacterCoords(int cellId, bool toBottom)
        {
            Vector2 cellCoords = cells[cellId].transform.localPosition;

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

            return charCoords;
        }

        // TODO: Add more than 2 characters on cell support
        public void SpawnCharacter(int cellId, int characterId, StuffClass stuffClass,
            int power, bool toBottom)
        {
            // Calculating the coordinates of character
            Vector2 charCoords = GetCharacterCoords(cellId, toBottom);

            // Spawning the character
            GameObject charPrefab = Instantiate(
                charGameObjects[stuffClass], transform, false);
            charPrefab.transform.localPosition = charCoords;

            // Setting the power
            Text powerText = charPrefab.GetComponentInChildren<Text>();
            powerText.text = "" + power;

            CharacterInfo charInfo = new CharacterInfo(characterId, toBottom, charPrefab);

            // Adding character to the board info
            if (!characters.ContainsKey(cellId))
            {
                characters.Add(cellId, new List<CharacterInfo>());
            }

            characters[cellId].Add(charInfo);

            charsCells.Add(characterId, cellId);
        }

        // TODO: Add more than 2 characters on cell support
        public void RemoveCharacter(int characterId)
        {
            CharacterInfo character = FindCharacter(characterId);

            // Destroying the character
            Destroy(character.charPrefab);

            // Removing character from structure
            characters[charsCells[characterId]].Remove(character);
        }

        public void ChangeCharacterPower(int characterId, int newPower)
        {
            CharacterInfo character = FindCharacter(characterId);
            Text powerText = character.charPrefab.GetComponentInChildren<Text>();
            powerText.text = "" + newPower;
        }
    }
}