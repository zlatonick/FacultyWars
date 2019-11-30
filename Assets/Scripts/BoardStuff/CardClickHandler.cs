using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardStuff
{
    public class CardClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Canvas canvas;

        private GameObject biggerPrefab;

        private GameObject dragPrefab;

        private int cardId;

        private bool isDraggingNow;
        private bool canEnlarge;

        private Action<int> cardPlayedAction;
        private Func<int, bool> canDragNow;

        void Start()
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 biggerScale = new Vector2(rect.localScale.x * 4, rect.localScale.y * 4);

            float bigCardWidth = rect.sizeDelta.x * biggerScale.x;
            float bigCardHeight = rect.sizeDelta.y * biggerScale.y;

            Vector2 biggerPos = new Vector2(
                rect.transform.localPosition.x,// - bigCardWidth / 2.5f,
                rect.transform.localPosition.y + bigCardHeight / 1.5f);

            isDraggingNow = false;

            if (gameObject.name == "Dragging")
            {
                canEnlarge = false;
            }
            else
            {
                // Creating the prefab with big view
                biggerPrefab = Instantiate(gameObject, transform.parent, false);

                biggerPrefab.transform.localScale = biggerScale;
                biggerPrefab.transform.localPosition = biggerPos;
                biggerPrefab.SetActive(false);

                canEnlarge = true;
            }
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetId(int id)
        {
            cardId = id;
        }

        public void SetCanDragPredicate(Func<int, bool> canDragNow)
        {
            this.canDragNow = canDragNow;
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

        void OnDestroy()
        {
            Destroy(biggerPrefab);
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
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDraggingNow)
            {
                dragPrefab.transform.localPosition =
                    canvas.ScreenToCanvasPosition(eventData.position);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDraggingNow)
            {
                isDraggingNow = false;
                canEnlarge = true;
                Destroy(dragPrefab);
                cardPlayedAction(cardId);
            }
        }
    }
}