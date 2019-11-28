using UnityEngine;
using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public class BoardStuffManager : MonoBehaviour
    {
        // Additional classes to store cell and character info
        class CellEffect
        {
            public int effectsQuan;

            public GameObject icon;

            public GameObject sign;
            public GameObject firstDigit;
            public GameObject secondDigit;

            public GameObject icon_2;

            public GameObject sign_2;
            public GameObject firstDigit_2;
            public GameObject secondDigit_2;

            public CellEffect()
            {
                effectsQuan = 0;
            }

            public CellEffect(GameObject icon, GameObject sign,
                GameObject firstDigit, GameObject secondDigit)
            {
                effectsQuan = 1;
                this.icon = icon;
                this.sign = sign;
                this.firstDigit = firstDigit;
                this.secondDigit = secondDigit;
            }

            public CellEffect(GameObject icon, GameObject sign, GameObject firstDigit,
                GameObject secondDigit, GameObject icon_2, GameObject sign_2, GameObject firstDigit_2,
                GameObject secondDigit_2)
            {
                effectsQuan = 2;
                this.icon = icon;
                this.sign = sign;
                this.firstDigit = firstDigit;
                this.secondDigit = secondDigit;
                this.icon_2 = icon_2;
                this.sign_2 = sign_2;
                this.firstDigit_2 = firstDigit_2;
                this.secondDigit_2 = secondDigit_2;
            }
        }

        class CharacterInfo
        {
            public int id;

            public bool isBottom;

            public GameObject charPrefab;

            public List<GameObject> digits;

            public CharacterInfo(int id, bool isBottom, GameObject charPrefab, List<GameObject> digits)
            {
                this.id = id;
                this.isBottom = isBottom;
                this.charPrefab = charPrefab;
                this.digits = digits;
            }
        }

        private float boardWidth;
        private float boardHeight;

        // ---------------- Cells

        public GameObject cellClosedPrefab;

        public GameObject cellOpenedPrefab;

        // Faculty icons game objects
        public GameObject iasaIcon;

        public GameObject fictIcon;

        public GameObject fpmIcon;

        // Additional effect icons
        public GameObject plusIcon;

        public GameObject minusIcon;

        public GameObject zeroIcon;

        public GameObject oneIcon;

        public GameObject twoIcon;

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

        private float digitsDistance;   // Distance between the centers of two digits

        // Digits game objects
        public GameObject[] powerDigits;

        // Character game objects (holded in structure)
        private Dictionary<StuffClass, GameObject> charGameObjects;

        // Characters on cells
        private Dictionary<int, List<CharacterInfo>> characters;

        // Inforamtion about characters' cells (charId -> cellId)
        private Dictionary<int, int> charsCells;

        void Start()
        {
            RectTransform thisRect = gameObject.GetComponent<RectTransform>();
            boardWidth = thisRect.sizeDelta.x * thisRect.localScale.x;
            boardHeight = thisRect.sizeDelta.y * thisRect.localScale.y;

            // Cells
            var cellPrefabRect = cellClosedPrefab.GetComponent<RectTransform>();
            cellWidth = cellPrefabRect.sizeDelta.x * cellPrefabRect.localScale.x;
            cellHeight = cellPrefabRect.sizeDelta.y * cellPrefabRect.localScale.y;

            cellOffset = 50;
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

            characterOffset = 10;

            var digitPrefabRect = powerDigits[0].GetComponent<RectTransform>();
            digitsDistance = digitPrefabRect.sizeDelta.x * digitPrefabRect.localScale.x;
        }

        public void FillBoardWithCells(int pairCellsQuan)
        {
            for (int i = 0; i < pairCellsQuan; i++)
            {
                // Bottom cell
                Vector2 bottomCellPos = new Vector2(
                    -cellWidth / 2 + cellWidthClear * (i - 1),
                    -cellHeight / 2);

                GameObject bottomCell = Instantiate(cellClosedPrefab, transform, false);
                bottomCell.transform.localPosition = bottomCellPos;
                cells.Add(2 * i + 1, bottomCell);

                // Top cell
                Vector2 topCellPos = new Vector2(bottomCellPos.x + cellOffset, cellHeight / 2);

                GameObject topCell = Instantiate(cellClosedPrefab, transform, false);
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

        private GameObject GetIconOfSign(bool sign)
        {
            return sign ? plusIcon : minusIcon;
        }

        private GameObject GetIconOfDigit(int digit)
        {
            switch (digit)
            {
                case 0:
                    return zeroIcon;

                case 1:
                    return oneIcon;

                case 2:
                    return twoIcon;
            }
            return null;
        }

        public void OpenCell(int cellId)
        {
            GameObject newCell = Instantiate(cellOpenedPrefab, transform, false);
            newCell.transform.localPosition = cells[cellId].transform.localPosition;

            Destroy(cells[cellId]);

            cells[cellId] = newCell;
        }

        public void SetEffect(int cellId, StuffClass stuffClass, int power)
        {
            // Instantiating all the elements
            GameObject stuffClassIcon = Instantiate(GetIconOfStuffClass(stuffClass),
                transform, false);
            GameObject signIcon = Instantiate(GetIconOfSign(power > 0), transform, false);
            GameObject firstDigitIcon = Instantiate(GetIconOfDigit(Mathf.Abs(power) / 10),
                transform, false);
            GameObject secondDigitIcon = Instantiate(zeroIcon, transform, false);

            // Setting the positions
            Vector2 cellPos = cells[cellId].transform.localPosition;

            stuffClassIcon.transform.localPosition = new Vector2(
                cellPos.x - 0.272f * cellWidth, cellPos.y);
            signIcon.transform.localPosition = new Vector2(
                cellPos.x - 0.0388f * cellWidth, cellPos.y);
            firstDigitIcon.transform.localPosition = new Vector2(
                cellPos.x - 0.1747f * cellWidth, cellPos.y);
            secondDigitIcon.transform.localPosition = new Vector2(
                cellPos.x - 0.2913f * cellWidth, cellPos.y);

            cellEffects.Add(cellId, new CellEffect(stuffClassIcon,
                signIcon, firstDigitIcon, secondDigitIcon));
        }

        public void RemoveEffect(int cellId)
        {
            if (!cellEffects.ContainsKey(cellId)) return;

            CellEffect effect = cellEffects[cellId];

            if (effect.effectsQuan > 0)
            {
                Destroy(effect.icon);
                Destroy(effect.sign);
                Destroy(effect.firstDigit);
                Destroy(effect.secondDigit);

                if (effect.effectsQuan > 1)
                {
                    Destroy(effect.icon_2);
                    Destroy(effect.sign_2);
                    Destroy(effect.firstDigit_2);
                    Destroy(effect.secondDigit_2);
                }
            }

            cellEffects.Remove(cellId);
        }

        public void RemoveCell(int cellId)
        {
            RemoveEffect(cellId);
            Destroy(cells[cellId]);
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

        private List<Vector2> GetCharDigitsCoords(Vector2 charCoords, int digitsQuan)
        {
            List<Vector2> result = new List<Vector2>();

            if (digitsQuan == 1)
            {
                result.Add(new Vector2(charCoords.x + (characterOffset / 2), charCoords.y));
            }
            else if (digitsQuan == 2)
            {
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) + digitsDistance / 2,
                    charCoords.y, -8));
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) - digitsDistance / 2,
                    charCoords.y, -8));
            }
            else if (digitsQuan == 3)
            {
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) + digitsDistance,
                    charCoords.y, -8));
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2),
                    charCoords.y, -8));
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) - digitsDistance,
                    charCoords.y, -8));
            }

            return result;

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

            // Getting the digits
            List<GameObject> charDigits = new List<GameObject>();

            int digitsQuan = (int)Mathf.Log10(power) + 1;
            int currPower = power;

            foreach (Vector2 coords in GetCharDigitsCoords(charCoords, digitsQuan))
            {
                GameObject digit = Instantiate(powerDigits[currPower % 10], transform, false);
                digit.transform.localPosition = coords;
                charDigits.Add(digit);

                currPower /= 10;
            }

            CharacterInfo charInfo = new CharacterInfo(characterId, toBottom, charPrefab, charDigits);

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
            foreach (GameObject obj in character.digits)
            {
                Destroy(obj);
            }
            Destroy(character.charPrefab);

            // Removing character from structure
            characters[charsCells[characterId]].Remove(character);
        }

        public void ChangeCharacterPower(int characterId, int newPower)
        {
            CharacterInfo character = FindCharacter(characterId);

            // Destroying old digits
            foreach (GameObject obj in character.digits)
            {
                Destroy(obj);
            }

            character.digits.Clear();

            // Drawing new digits
            int digitsQuan = (int)Mathf.Log10(newPower) + 1;
            int currPower = newPower;

            foreach (Vector2 coords in GetCharDigitsCoords(
                character.charPrefab.transform.localPosition, digitsQuan))
            {
                GameObject digit = Instantiate(powerDigits[currPower % 10], transform, false);
                digit.transform.localPosition = coords;
                character.digits.Add(digit);

                currPower /= 10;
            }
        }
    }
}