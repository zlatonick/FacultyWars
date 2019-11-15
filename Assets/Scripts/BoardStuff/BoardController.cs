using System.Collections.Generic;
using UnityEngine;

namespace BoardStuff
{    public class BoardController : MonoBehaviour
    {
        List<Cell> cells;

        // Start is called before the first frame update
        void Start()
        {
            // Creating all the cells using child gameobjects
            cells = new List<Cell>();

            foreach (Transform tran in transform)
            {
                cells.Add(new CellImpl(cells.Count, tran.gameObject));
            }
        }


    }
}

