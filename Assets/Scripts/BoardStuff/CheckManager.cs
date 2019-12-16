using MetaInfo;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardStuff
{
    public class CheckManager : MonoBehaviour
    {
        // Levels are indexes. Levels: 0 - top, 1 - middle, 2 - bottom
        public CheckDragHandler[] redChecks;
        public CheckDragHandler[] blueChecks;
        public CheckDragHandler[] greenChecks;

        public Canvas canvas;

        private CheckDragHandler[] currChecks;

        private Dictionary<int, int> checksQuan;        // level -> quantity

        // Actions
        private Func<Vector2, Cell> cellInThePlace;

        private Predicate<Cell> canPlaceCheckThere;

        private Action<Cell, int> dragFinishedAction;

        private Action<int> checkClickedAction;

        private Func<List<Cell>> getPlacableCells;
        private Action<List<Cell>> highlightPlacableCells;
        private Action unhighlightCells;

        void Start()
        {
            for (int i = 0; i < redChecks.Length; i++)
            {
                redChecks[i].SetLevel(i);
                redChecks[i].SetCanvas(canvas);
                redChecks[i].SetChecksCount(1);
                redChecks[i].gameObject.SetActive(false);
                redChecks[i].SetDragStartedAction(OnDragStarted);
                redChecks[i].SetDragFinishedAction(OnDragFinished);
                redChecks[i].SetClickedAction(OnCheckClicked);
            }

            for (int i = 0; i < blueChecks.Length; i++)
            {
                blueChecks[i].SetLevel(i);
                blueChecks[i].SetCanvas(canvas);
                blueChecks[i].SetChecksCount(1);
                blueChecks[i].gameObject.SetActive(false);
                blueChecks[i].SetDragStartedAction(OnDragStarted);
                blueChecks[i].SetDragFinishedAction(OnDragFinished);
                blueChecks[i].SetClickedAction(OnCheckClicked);
            }

            for (int i = 0; i < greenChecks.Length; i++)
            {
                greenChecks[i].SetLevel(i);
                greenChecks[i].SetCanvas(canvas);
                greenChecks[i].SetChecksCount(1);
                greenChecks[i].gameObject.SetActive(false);
                greenChecks[i].SetDragStartedAction(OnDragStarted);
                greenChecks[i].SetDragFinishedAction(OnDragFinished);
                greenChecks[i].SetClickedAction(OnCheckClicked);
            }

            checksQuan = new Dictionary<int, int>();
            checksQuan.Add(0, 0);
            checksQuan.Add(1, 0);
            checksQuan.Add(2, 0);

            currChecks = null;
        }

        public void SetStuffClass(StuffClass stuffClass, Dictionary<int, int> powers)
        {
            if (stuffClass == StuffClass.IASA) currChecks = redChecks;
            else if (stuffClass == StuffClass.FICT) currChecks = greenChecks;
            else if (stuffClass == StuffClass.FPM) currChecks = blueChecks;

            foreach (var pr in powers)
            {
                currChecks[pr.Key].SetPower(pr.Value);
            }
        }

        public void SetCanDrag(bool canDragNow)
        {
            foreach (CheckDragHandler check in currChecks)
            {
                check.SetCanDrag(canDragNow);
            }
        }

        public void SetCellInThePlacePredicate(Func<Vector2, Cell> cellInThePlace)
        {
            this.cellInThePlace = cellInThePlace;
        }

        public void SetCanPlaceCheckThere(Predicate<Cell> canPlaceCheckThere)
        {
            this.canPlaceCheckThere = canPlaceCheckThere;
        }

        public void SetDragFinishedAction(Action<Cell, int> dragFinishedAction)
        {
            this.dragFinishedAction = dragFinishedAction;
        }

        public void SetCheckClickedAction(Action<int> checkClickedAction)
        {
            this.checkClickedAction = checkClickedAction;
        }

        public void SetPlacableCellsFunctions(Func<List<Cell>> getPlacableCells,
            Action<List<Cell>> highlightPlacableCells, Action unhighlightCells)
        {
            this.getPlacableCells = getPlacableCells;
            this.highlightPlacableCells = highlightPlacableCells;
            this.unhighlightCells = unhighlightCells;
        }

        private void OnDragStarted()
        {
            highlightPlacableCells(getPlacableCells());
        }

        private void OnDragFinished(CheckDragHandler check, Vector2 pos)
        {
            // Unhighlight the cells
            unhighlightCells();

            Cell cell = cellInThePlace(pos);

            int checkLevel = check.GetLevel();

            if (cell != null && canPlaceCheckThere(cell))
            {
                // Removing the check
                RemoveCheck(checkLevel);

                dragFinishedAction(cell, checkLevel);
            }
            else
            {
                Debug.Log("Wrong cell to spawn a character");
            }
        }

        public void OnCheckClicked(CheckDragHandler check)
        {
            checkClickedAction(check.GetLevel());
        }

        // Level: 0 - top, 1 - middle, 2 - bottom
        public void AddCheck(int level)
        {
            CheckDragHandler currCheck = currChecks[level];

            if (checksQuan[level] == 0)
            {
                checksQuan[level]++;
                currCheck.gameObject.SetActive(true);
                currCheck.SetChecksCount(1);
            }
            else
            {
                checksQuan[level]++;
                currCheck.SetChecksCount(checksQuan[level]);
            }
        }

        public void RemoveCheck(int checkLevel)
        {
            CheckDragHandler check = currChecks[checkLevel];

            if (checksQuan[checkLevel] > 1)
            {
                checksQuan[checkLevel]--;
                check.SetChecksCount(checksQuan[checkLevel]);
            }
            else
            {
                checksQuan[checkLevel]--;
                check.gameObject.SetActive(false);
            }
        }
    }
}