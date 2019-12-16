using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CheckDragHandler : MonoBehaviour, IDragHandler,
        IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private Canvas canvas;

        private GameObject checkPrefab;

        public GameObject checksCount;
        private Text checksCountText;

        public Text powerText;

        private bool canDrag;

        private int level;

        private Action dragStartedAction;

        private Action<CheckDragHandler, Vector2> dragFinishedAction;

        private Action<CheckDragHandler> clickedAction;

        void Start()
        {
            checksCountText = checksCount.GetComponentInChildren<Text>();
            checkPrefab = null;
        }

        public void SetLevel(int level)
        {
            this.level = level;
        }

        public int GetLevel()
        {
            return level;
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetCanDrag(bool canDrag)
        {
            this.canDrag = canDrag;
        }

        public void SetPower(int power)
        {
            powerText.text = "" + power;
        }

        public void SetChecksCount(int count)
        {
            if (count == 1)
            {
                checksCount.SetActive(false);
            }
            else
            {
                checksCount.SetActive(true);
                checksCountText.text = "" + count;
            }
        }

        public void SetDragStartedAction(Action dragStartedAction)
        {
            this.dragStartedAction = dragStartedAction;
        }

        public void SetDragFinishedAction(Action<CheckDragHandler, Vector2> dragFinishedAction)
        {
            this.dragFinishedAction = dragFinishedAction;
        }

        public void SetClickedAction(Action<CheckDragHandler> clickedAction)
        {
            this.clickedAction = clickedAction;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                checkPrefab = Instantiate(gameObject, transform.parent.parent, false);
                checkPrefab.transform.localPosition =
                    canvas.ScreenToCanvasPosition(eventData.position);

                checkPrefab.transform.Find("ChecksCount").gameObject.SetActive(false);

                dragStartedAction();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                if (checkPrefab == null)
                {
                    OnBeginDrag(eventData);
                }

                checkPrefab.transform.localPosition =
                    canvas.ScreenToCanvasPosition(eventData.position);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                Destroy(checkPrefab);
                checkPrefab = null;
                dragFinishedAction(this, canvas.ScreenToCanvasPosition(eventData.position));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickedAction(this);
        }
    }
}