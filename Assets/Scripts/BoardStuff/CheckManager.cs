using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardStuff
{
    public class CheckManager : MonoBehaviour
    {
        public CheckDragHandler topCheck;
        public CheckDragHandler middleCheck;
        public CheckDragHandler bottomCheck;

        public Canvas canvas;

        private Dictionary<CheckDragHandler, int> checksQuan;

        private Func<Vector2, Cell> cellInThePlace;
        private Action<Cell, int> dragFinishedAction;

        void Start()
        {
            // DEBUG
            SetCanDrag(true);

            topCheck.SetCanvas(canvas);
            middleCheck.SetCanvas(canvas);
            bottomCheck.SetCanvas(canvas);

            topCheck.SetChecksCount(1);
            middleCheck.SetChecksCount(1);
            bottomCheck.SetChecksCount(1);

            topCheck.gameObject.SetActive(false);
            middleCheck.gameObject.SetActive(false);
            bottomCheck.gameObject.SetActive(false);

            checksQuan = new Dictionary<CheckDragHandler, int>();
            checksQuan.Add(topCheck, 0);
            checksQuan.Add(middleCheck, 0);
            checksQuan.Add(bottomCheck, 0);

            topCheck.SetDragFinishedAction(OnDragFinished);
            middleCheck.SetDragFinishedAction(OnDragFinished);
            bottomCheck.SetDragFinishedAction(OnDragFinished);
        }

        public void SetCanDrag(bool canDragNow)
        {
            topCheck.SetCanDrag(canDragNow);
            middleCheck.SetCanDrag(canDragNow);
            bottomCheck.SetCanDrag(canDragNow);
        }

        public void SetCellInThePlacePredicate(Func<Vector2, Cell> cellInThePlace)
        {
            this.cellInThePlace = cellInThePlace;
        }

        public void SetDragFinishedAction(Action<Cell, int> dragFinishedAction)
        {
            this.dragFinishedAction = dragFinishedAction;
        }

        private void OnDragFinished(CheckDragHandler check, Vector2 pos)
        {
            Cell cell = cellInThePlace(pos);

            if (cell != null)
            {
                Debug.Log("Spawning a character on cell " + cell.GetId());

                // Removing the check
                if (checksQuan[check] > 1)
                {
                    checksQuan[check]--;
                    check.SetChecksCount(checksQuan[check]);
                }
                else
                {
                    checksQuan[check]--;
                    check.gameObject.SetActive(false);
                }

                int checkLevel = 0;
                if (check == middleCheck) checkLevel = 1;
                else if (check == bottomCheck) checkLevel = 2;

                dragFinishedAction(cell, checkLevel);
            }
            else
            {
                Debug.Log("Wrong cell to spawn a character");
            }
        }

        // Level: 0 - top, 1 - middle, 2 - bottom
        public void AddCheck(int level)
        {
            CheckDragHandler currCheck;

            if (level == 0) currCheck = topCheck;
            else if (level == 1) currCheck = middleCheck;
            else currCheck = bottomCheck;

            if (checksQuan[currCheck] == 0)
            {
                checksQuan[currCheck]++;
                currCheck.gameObject.SetActive(true);
                currCheck.SetChecksCount(1);
            }
            else
            {
                checksQuan[currCheck]++;
                currCheck.SetChecksCount(checksQuan[currCheck]);
            }
        }
    }
}