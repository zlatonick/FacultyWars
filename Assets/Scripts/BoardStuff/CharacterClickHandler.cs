using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardStuff
{
    public class CharacterClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Canvas canvas;

        private Action<CharacterClickHandler, Vector2> clickAction;

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetClickAction(Action<CharacterClickHandler, Vector2> clickAction)
        {
            this.clickAction = clickAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickAction(this, canvas.ScreenToCanvasPosition(eventData.position));
        }
    }
}
