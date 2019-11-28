using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CheckDragHandler : MonoBehaviour, IDragHandler,
        IBeginDragHandler, IEndDragHandler
    {
        private Canvas canvas;

        private GameObject checkPrefab;

        public GameObject checksCount;
        private Text checksCountText;

        private bool canDrag;

        private Action<CheckDragHandler, Vector2> dragFinishedAction;

        void Start()
        {
            checksCountText = checksCount.GetComponentInChildren<Text>();
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetCanDrag(bool canDrag)
        {
            this.canDrag = canDrag;
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

        public void SetDragFinishedAction(Action<CheckDragHandler, Vector2> dragFinishedAction)
        {
            this.dragFinishedAction = dragFinishedAction;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                checkPrefab = Instantiate(gameObject, transform.parent.parent, false);
                checkPrefab.transform.localPosition =
                    canvas.ScreenToCanvasPosition(eventData.position);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                checkPrefab.transform.localPosition =
                    canvas.ScreenToCanvasPosition(eventData.position);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                Destroy(checkPrefab);
                dragFinishedAction(this, canvas.ScreenToCanvasPosition(eventData.position));
                //dragFinishedAction(this, eventData.position);
            }
        }
    }
}