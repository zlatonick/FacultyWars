using UnityEngine;
using System.Collections.Generic;

namespace BoardStuff
{
    public class BoardClickHandler : MonoBehaviour
    {
        public int pairCellsQuan;

        public GameObject cellPrefab;

        private List<GameObject> cells;

        void Start()
        {
            var cellPrefabRect = cellPrefab.GetComponent<RectTransform>();
            float cellWidth = cellPrefabRect.sizeDelta.x * cellPrefabRect.localScale.x;
            float cellHeight = cellPrefabRect.sizeDelta.y * cellPrefabRect.localScale.y;

            float cellOffset = 32;
            float cellWidthClear = cellWidth - cellOffset;

            cells = new List<GameObject>();

            for (int i = 0; i < pairCellsQuan; i++)
            {
                // Bottom cell
                Vector2 bottomCellPos = new Vector2(
                    -cellWidth / 2 + cellWidthClear * (i - 1),
                    -cellHeight / 2);

                GameObject bottomCell = Instantiate(cellPrefab, transform, false);
                bottomCell.transform.localPosition = bottomCellPos;
                cells.Add(bottomCell);

                // Top cell
                Vector2 topCellPos = new Vector2(bottomCellPos.x + cellOffset, cellHeight / 2);

                GameObject topCell = Instantiate(cellPrefab, transform, false);
                topCell.transform.localPosition = topCellPos;
                cells.Add(topCell);
            }
        }

        void FixedUpdate()
        {
            //Debug.Log("Mouse: " + Input.mousePosition.x + " " + Input.mousePosition.y);
        }
    }
}