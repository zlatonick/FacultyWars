using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardStuff
{
    public class CardClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Canvas canvas;

        private RectTransform rect;

        private GameObject biggerPrefab;

        private GameObject dragPrefab;

        private int cardId;

        private float smallCardWidth;

        private float bigCardWidth;
        private float bigCardHeight;

        private bool isDraggingNow;
        private bool canEnlarge;

        private Action<int> dragStartedAction;
        private Action<int> cardPlayedAction;
        private Func<int, bool> canDragNow;

        public void InitCard()
        {
            rect = GetComponent<RectTransform>();

            smallCardWidth = rect.sizeDelta.x * rect.localScale.x;

            Vector2 biggerScale = new Vector2(rect.localScale.x * 4, rect.localScale.y * 4);

            bigCardWidth = rect.sizeDelta.x * biggerScale.x;
            bigCardHeight = rect.sizeDelta.y * biggerScale.y;

            Vector2 biggerPos = new Vector2(
                rect.transform.localPosition.x - bigCardWidth / 2.5f,
                rect.transform.localPosition.y + bigCardHeight / 1.5f);

            isDraggingNow = false;
            
            biggerPrefab = Instantiate(gameObject, transform.parent, false);

            // Removing the script component
            Destroy(biggerPrefab.GetComponent<CardClickHandler>());

            biggerPrefab.transform.localScale = biggerScale;
            biggerPrefab.transform.localPosition = biggerPos;
            biggerPrefab.SetActive(false);

            canEnlarge = true;
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetId(int id)
        {
            cardId = id;
        }

        public void SetCanEnlarge(bool canEnlarge)
        {
            this.canEnlarge = canEnlarge;
        }

        public void SetCanDragPredicate(Func<int, bool> canDragNow)
        {
            this.canDragNow = canDragNow;
        }

        public void SetDragStartedAction(Action<int> dragStartedAction)
        {
            this.dragStartedAction = dragStartedAction;
        }

        public void SetCardPlayedAction(Action<int> cardPlayedAction)
        {
            this.cardPlayedAction = cardPlayedAction;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (canEnlarge)
            {
                biggerPrefab.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (canEnlarge)
            {
                biggerPrefab.SetActive(false);
            }
        }

        public void FixBigPrefabPosition()
        {
            Vector2 biggerPos = new Vector2(
                rect.transform.localPosition.x - bigCardWidth / 2.5f,
                rect.transform.localPosition.y + bigCardHeight / 1.5f);

            biggerPrefab.transform.localPosition = biggerPos;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canDragNow(cardId))
            {
                canEnlarge = false;
                isDraggingNow = true;
                biggerPrefab.SetActive(false);

                dragPrefab = Instantiate(gameObject, transform.parent.parent, false);
                dragPrefab.transform.localPosition =
                    canvas.ScreenToCanvasPosition(eventData.position);
                dragPrefab.name = "Dragging";

                Image cardImage = gameObject.GetComponentInChildren<Image>();
                cardImage.color = Color.clear;
                dragStartedAction(cardId);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDraggingNow)
            {
                Vector2 mousePos = canvas.ScreenToCanvasPosition(eventData.position);
                dragPrefab.transform.localPosition = new Vector2(
                    mousePos.x - (smallCardWidth / 2), mousePos.y);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDraggingNow)
            {
                isDraggingNow = false;
                canEnlarge = true;
                Destroy(dragPrefab);
                Destroy(biggerPrefab);
                cardPlayedAction(cardId);
            }
        }
    }
}