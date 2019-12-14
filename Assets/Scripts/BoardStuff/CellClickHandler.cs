using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardStuff
{
    public class CellClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Canvas canvas;

        private Action<Vector2> clickAction;

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetClickAction(Action<Vector2> clickAction)
        {
            this.clickAction = clickAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickAction(canvas.ScreenToCanvasPosition(eventData.position));
        }
    }
}
