using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardStuff
{
    public class CardClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private GameObject biggerPrefab;

        void Start()
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 biggerScale = new Vector2(rect.localScale.x * 4, rect.localScale.y * 4);

            float bigCardWidth = rect.sizeDelta.x * biggerScale.x;
            float bigCardHeight = rect.sizeDelta.y * biggerScale.y;

            Vector2 biggerPos = new Vector2(
                rect.transform.localPosition.x - bigCardWidth / 2.5f,
                rect.transform.localPosition.y + bigCardHeight / 1.5f);

            // Creating the prefab with big view
            biggerPrefab = Instantiate(gameObject, transform.parent, false);

            biggerPrefab.transform.localScale = biggerScale;
            biggerPrefab.transform.localPosition = biggerPos;
            biggerPrefab.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            biggerPrefab.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            biggerPrefab.SetActive(false);
        }

        void OnDestroy()
        {
            Destroy(biggerPrefab);
        }
    }
}