using GameStuff;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BoardStuff
{
    public class ChooserImpl : MonoBehaviour, Chooser
    {
        public Text messageUI;

        private bool waitForCell;
        private bool waitForCharacter;
        private bool waitForCheck;

        private Action<Cell> cellAction;
        private Action<Character> characterAction;
        private Action<Check> checkAction;

        public void ChooseCell(string message, Action<Cell> action)
        {
            messageUI.text = message;
            waitForCell = true;
            cellAction = action;
        }

        public void ChooseCharacter(string message, Action<Character> action)
        {
            messageUI.text = message;
            waitForCharacter = true;
            characterAction = action;
        }

        public void ChooseCheck(string message, Action<Check> action)
        {
            messageUI.text = message;
            waitForCheck = true;
            checkAction = action;
        }

        public void CellClicked(Cell cell)
        {
            if (waitForCell)
            {
                waitForCell = false;
                messageUI.text = "";
                cellAction(cell);
            }
        }

        public void CharacterClicked(Character character)
        {
            if (waitForCharacter)
            {
                waitForCharacter = false;
                messageUI.text = "";
                characterAction(character);
            }
        }

        public void CheckClicked(Check check)
        {
            if (waitForCheck)
            {
                waitForCheck = false;
                messageUI.text = "";
                checkAction(check);
            }
        }
    }
}