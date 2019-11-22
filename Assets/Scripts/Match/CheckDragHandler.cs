using UnityEngine;
using UnityEngine.EventSystems;

namespace Match
{
    public class CheckDragHandler : MonoBehaviour, IDragHandler,
        IBeginDragHandler, IEndDragHandler
    {
        public GameController gameController;

        public int power;

        private Transform checkPrefab;

        public void OnBeginDrag(PointerEventData eventData)
        {
            checkPrefab = Instantiate(transform, Input.mousePosition, Quaternion.identity);
        }

        public void OnDrag(PointerEventData eventData)
        {
            checkPrefab.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //gameController
        }
    }
}