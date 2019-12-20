using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CellClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Canvas canvas;

        private GameObject cellEffectPanel;
        public GameObject[] effectsStraight;

        private GameObject cellEffectPanelInst;
        private List<GameObject> effectsStraightInst;

        private Action<Vector2> clickAction;

        private bool effectPanelIsShowed;

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;

            effectsStraightInst = new List<GameObject>();

            effectPanelIsShowed = false;
        }

        public void SetCellEffectPanel(GameObject cellEffectPanel)
        {
            this.cellEffectPanel = cellEffectPanel;
        }

        public void SetEffectsStraight(GameObject[] effectsStraight)
        {
            this.effectsStraight = effectsStraight;
        }

        public void SetClickAction(Action<Vector2> clickAction)
        {
            this.clickAction = clickAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickAction(canvas.ScreenToCanvasPosition(eventData.position));
        }

        public void ShowHideEffectPanel(BoardStuffManager.CellEffect cellEffect)
        {
            if (!effectPanelIsShowed)
            {
                cellEffectPanelInst = Instantiate(cellEffectPanel, transform, false);
                cellEffectPanelInst.transform.localPosition = new Vector2(22, 110);

                if (cellEffect.EffectsQuan == 1)
                {
                    GameObject effectIcon = Instantiate(
                        effectsStraight[(int)cellEffect.StuffClass],
                        cellEffectPanelInst.transform, false);

                    effectIcon.transform.localPosition = new Vector2(-11, 0);

                    GameObject textObj = cellEffectPanelInst.transform.Find("TextMiddle").gameObject;
                    Text text = textObj.GetComponent<Text>();
                    text.text = "" + (cellEffect.Power > 0 ? "+" : "") + cellEffect.Power;

                    effectsStraightInst.Add(effectIcon);
                }
                else if (cellEffect.EffectsQuan == 2)
                {
                    GameObject effectIcon1 = Instantiate(
                        effectsStraight[(int)cellEffect.StuffClass],
                        cellEffectPanelInst.transform, false);

                    GameObject effectIcon2 = Instantiate(
                        effectsStraight[(int)cellEffect.StuffClass2],
                        cellEffectPanelInst.transform, false);

                    GameObject textLeftObj = cellEffectPanelInst.transform.Find("TextLeft").gameObject;
                    Text textLeft = textLeftObj.GetComponent<Text>();
                    textLeft.text = "" + (cellEffect.Power > 0 ? "+" : "") + cellEffect.Power;

                    GameObject textRightObj = cellEffectPanelInst.transform.Find("TextRight").gameObject;
                    Text textRight = textRightObj.GetComponent<Text>();
                    textRight.text = "" + (cellEffect.Power2 > 0 ? "+" : "") + cellEffect.Power2;

                    effectIcon1.transform.localPosition = new Vector2(-31, 0);
                    effectIcon2.transform.localPosition = new Vector2(9, 0);

                    effectsStraightInst.Add(effectIcon1);
                    effectsStraightInst.Add(effectIcon2);
                }
            }
            else
            {
                Destroy(cellEffectPanelInst);

                foreach (GameObject obj in effectsStraightInst)
                {
                    Destroy(obj);
                }
                effectsStraightInst.Clear();
            }

            effectPanelIsShowed = !effectPanelIsShowed;
        }
    }
}
