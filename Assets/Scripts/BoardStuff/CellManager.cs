using MetaInfo;
using System.Collections.Generic;
using UnityEngine;

namespace BoardStuff
{
    public class CellManager : MonoBehaviour
    {
        // Class stores information about the current cell effects
        class CellEffect
        {
            public bool isSingle;

            public Transform icon;

            public Transform sign;
            public Transform firstDigit;
            public Transform secondDigit;

            public Transform icon_2;

            public Transform sign_2;
            public Transform firstDigit_2;
            public Transform secondDigit_2;

            public CellEffect(bool isSingle, Transform icon, Transform sign,
                Transform firstDigit, Transform secondDigit)
            {
                this.isSingle = isSingle;
                this.icon = icon;
                this.sign = sign;
                this.firstDigit = firstDigit;
                this.secondDigit = secondDigit;
            }

            public CellEffect(bool isSingle, Transform icon, Transform sign, Transform firstDigit,
                Transform secondDigit, Transform icon_2, Transform sign_2, Transform firstDigit_2,
                Transform secondDigit_2) : this(isSingle, icon, sign, firstDigit, secondDigit)
            {
                this.icon_2 = icon_2;
                this.sign_2 = sign_2;
                this.firstDigit_2 = firstDigit_2;
                this.secondDigit_2 = secondDigit_2;
            }
        }

        // Cell game objects
        public Transform cellClosedLight;

        public Transform cellClosedDark;

        public Transform cellOpenedLight;

        public Transform cellOpenedDark;

        // Faculty icons game objects
        public Transform iasaIcon;

        public Transform fictIcon;

        public Transform fpmIcon;

        // Additional effect icons
        public Transform plusIcon;

        public Transform minusIcon;

        public Transform zeroIcon;

        public Transform oneIcon;

        public Transform twoIcon;

        // Cell positions
        Dictionary<int, Vector3> cellPositions;

        // Cell prefabs
        Dictionary<int, Transform> cellPrefabs;

        // Cell effects
        Dictionary<int, CellEffect> cellEffects;

        void Start()
        {
            cellPrefabs = new Dictionary<int, Transform>();

            cellEffects = new Dictionary<int, CellEffect>();

            cellPositions = new Dictionary<int, Vector3>();
        }

        private static bool IsDark(int cellId)
        {
            return (cellId + 1) / 2 % 2 == 0;
        }

        private Transform GetIconOfStuffClass(StuffClass stuffClass)
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

        private Transform GetIconOfSign(bool sign)
        {
            return sign ? plusIcon : minusIcon;
        }

        private Transform GetIconOfDigit(int digit)
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

        public void SetStartPosition(int cellsQuan)
        {
            // Initializing the storage of cell coordinates
            cellPositions.Clear();

            for (int i = 0; i < cellsQuan; i++)
            {
                cellPositions.Add(i, BoardCoordinates.GetCellCoords(i));
            }

            // Creating the prefabs
            for (int i = 0; i < cellsQuan; i++)
            {
                if (IsDark(i))
                {
                    cellPrefabs.Add(i, Instantiate(
                        cellClosedDark, cellPositions[i], Quaternion.identity));
                }
                else
                {
                    cellPrefabs.Add(i, Instantiate(
                        cellClosedLight, cellPositions[i], Quaternion.identity));
                }
            }
        }

        public void ChangeCellPrefab(int cellId, bool opened)
        {
            Destroy(cellPrefabs[cellId]);

            if (opened)
            {
                if (IsDark(cellId))
                {
                    cellPrefabs[cellId] = Instantiate(
                        cellOpenedDark, cellPositions[cellId], Quaternion.identity);
                }
                else
                {
                    cellPrefabs[cellId] = Instantiate(
                        cellOpenedLight, cellPositions[cellId], Quaternion.identity);
                }
            }
            else
            {
                if (IsDark(cellId))
                {
                    cellPrefabs[cellId] = Instantiate(
                        cellClosedDark, cellPositions[cellId], Quaternion.identity);
                }
                else
                {
                    cellPrefabs[cellId] = Instantiate(
                        cellClosedLight, cellPositions[cellId], Quaternion.identity);
                }
            }
        }

        public void SetEffect(int cellId, StuffClass stuffClass, int power)
        {
            Vector3 cellPos = cellPositions[cellId];

            Vector3 facultyPos = new Vector3(
                cellPos.x - 0.272f * BoardCoordinates.cellWidth, cellPos.y, -7.5f);
            Vector3 signPos = new Vector3(
                cellPos.x + 0.0388f * BoardCoordinates.cellWidth, cellPos.y, -7.5f);
            Vector3 firstDigitPos = new Vector3(
                cellPos.x + 0.1747f * BoardCoordinates.cellWidth, cellPos.y, -7.5f);
            Vector3 secondDigitPos = new Vector3(
                cellPos.x + 0.2913f * BoardCoordinates.cellWidth, cellPos.y, -7.5f);

            Transform stuffClassIcon = Instantiate(GetIconOfStuffClass(stuffClass),
                facultyPos, Quaternion.identity);
            Transform signIcon = Instantiate(GetIconOfSign(power > 0),
                signPos, Quaternion.identity);
            Transform firstDigitIcon = Instantiate(GetIconOfDigit(Mathf.Abs(power) / 10),
                firstDigitPos, Quaternion.identity);
            Transform secondDigitIcon = Instantiate(zeroIcon, secondDigitPos, Quaternion.identity);

            cellEffects.Add(cellId, new CellEffect(true, stuffClassIcon,
                signIcon, firstDigitIcon, secondDigitIcon));
        }

        // TODO: Rewrite for relative coordinates
        public void SetEffect(int cellId, StuffClass stuffClass, int power,
            StuffClass stuffClass2, int power2)
        {
            Vector3 cellPos = cellPositions[cellId];

            Vector3 facultyPos = new Vector3(cellPos.x - 0.5f, cellPos.y + 0.85f, -7.5f);
            Vector3 signPos = new Vector3(cellPos.x + 0.3f, cellPos.y + 0.85f, -7.5f);
            Vector3 firstDigitPos = new Vector3(cellPos.x + 0.65f, cellPos.y + 0.85f, -7.5f);
            Vector3 secondDigitPos = new Vector3(cellPos.x + 0.95f, cellPos.y + 0.85f, -7.5f);

            Vector3 facultyPos2 = new Vector3(cellPos.x - 0.85f, cellPos.y - 0.85f, -7.5f);
            Vector3 signPos2 = new Vector3(cellPos.x - 0.05f, cellPos.y - 0.85f, -7.5f);
            Vector3 firstDigitPos2 = new Vector3(cellPos.x + 0.3f, cellPos.y - 0.85f, -7.5f);
            Vector3 secondDigitPos2 = new Vector3(cellPos.x + 0.6f, cellPos.y - 0.85f, -7.5f);

            Transform stuffClassIcon = Instantiate(GetIconOfStuffClass(stuffClass),
                facultyPos, Quaternion.identity);
            Transform signIcon = Instantiate(GetIconOfSign(power > 0),
                signPos, Quaternion.identity);
            Transform firstDigitIcon = Instantiate(GetIconOfDigit(Mathf.Abs(power) / 10),
                firstDigitPos, Quaternion.identity);
            Transform secondDigitIcon = Instantiate(zeroIcon, secondDigitPos, Quaternion.identity);

            Transform stuffClassIcon2 = Instantiate(GetIconOfStuffClass(stuffClass2),
                facultyPos2, Quaternion.identity);
            Transform signIcon2 = Instantiate(GetIconOfSign(power2 > 0),
                signPos2, Quaternion.identity);
            Transform firstDigitIcon2 = Instantiate(GetIconOfDigit(Mathf.Abs(power2) / 10),
                firstDigitPos2, Quaternion.identity);
            Transform secondDigitIcon2 = Instantiate(zeroIcon, secondDigitPos2, Quaternion.identity);

            cellEffects.Add(cellId, new CellEffect(false, stuffClassIcon,
                signIcon, firstDigitIcon, secondDigitIcon, stuffClassIcon2,
                signIcon2, firstDigitIcon2, secondDigitIcon2));
        }

        public void RemoveEffect(int cellId)
        {
            CellEffect effect = cellEffects[cellId];

            Destroy(effect.icon);
            Destroy(effect.sign);
            Destroy(effect.firstDigit);
            Destroy(effect.secondDigit);

            if (!effect.isSingle)
            {
                Destroy(effect.icon_2);
                Destroy(effect.sign_2);
                Destroy(effect.firstDigit_2);
                Destroy(effect.secondDigit_2);
            }

            cellEffects.Remove(cellId);
        }

        public void RemoveCell(int cellId)
        {
            Destroy(cellPrefabs[cellId]);
            cellPrefabs.Remove(cellId);
        }
    }
}