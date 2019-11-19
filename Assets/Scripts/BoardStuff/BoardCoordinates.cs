using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardStuff
{
    public class BoardCoordinates
    {
        public static readonly float cellWidth = 2.575f;

        public static readonly float cellHeight = 3.507f;

        public static readonly float cellOffsetX = 0.666f;

        public static readonly float characterOffset = 0.1f;

        public static readonly float digitsDistance = 0.12f;

        public static Vector3 GetCellCoords(int cellId)
        {
            float coord_y = cellId % 2 == 0 ? cellHeight / 2 : -cellHeight / 2;

            float coord_x = (cellId / 2 - 2) * cellWidth + (cellWidth / 2);

            if (cellId % 2 == 0)
            {
                coord_x += cellOffsetX / 2;
            }
            else
            {
                coord_x -= cellOffsetX / 2;
            }

            return new Vector3(coord_x, coord_y, -6);
        }

        // TODO: Add more than 2 characters on cell support
        public static Vector3 GetCharacterCoords(int cellId, bool isBottom)
        {
            Vector3 cellCoords = GetCellCoords(cellId);

            float coord_x = cellCoords.x - characterOffset;
            float coord_y = cellCoords.y;

            if (isBottom)
            {
                coord_x -= cellOffsetX / 4;
                coord_y -= cellHeight / 4;
            }
            else
            {
                coord_x += cellOffsetX / 4;
                coord_y += cellHeight / 4;
            }

            return new Vector3(coord_x, coord_y, -7);
        }

        // Returns coordinates of digits that define the power of a character.
        // Digits go in the order from the right to the left
        public static List<Vector3> GetCharDigitsCoords(Vector3 charCoords, int digitsQuan)
        {
            List<Vector3> result = new List<Vector3>();

            if (digitsQuan == 1)
            {
                result.Add(new Vector3(charCoords.x + (characterOffset / 2), charCoords.y, -8));
            }
            else if (digitsQuan == 2)
            {
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) + digitsDistance,
                    charCoords.y, -8));
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) - digitsDistance,
                    charCoords.y, -8));
            }
            else if (digitsQuan == 3)
            {
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) + digitsDistance * 2,
                    charCoords.y, -8));
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2),
                    charCoords.y, -8));
                result.Add(new Vector3(
                    charCoords.x + (characterOffset / 2) - digitsDistance * 2,
                    charCoords.y, -8));
            }

            return result;
        }
    }
}