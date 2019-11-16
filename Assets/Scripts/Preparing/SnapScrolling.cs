using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{

	[Range(1, 50)]
	[Header("Controllers")]
	public int cardCount;

    [Range(0, 500)]
	public int cardOffset;

    [Range(0f, 20f)]
	public float snapSpeed;

    [Range(0f, 5f)]
	public float scaleOffset;

	[Header("Other objects")]
	public GameObject cardPrefab;
	public ScrollRect scrollRect;

	private GameObject[] instCards;
	private Vector2[] cardsPos;
	private Vector2[] cardsScale;

	private RectTransform contentRect;
	private Vector2 contentVector;

	private int selectedCardId;
	private bool isScrolling;

	private void Start()
    {
		contentRect = GetComponent<RectTransform>();
		instCards = new GameObject[cardCount];
		cardsPos = new Vector2[cardCount];
		cardsScale = new Vector2[cardCount];

		for (int i = 0; i < cardCount; i++)
		{
			instCards[i] = Instantiate(cardPrefab, transform, false);
			if (i == 0) continue;

			instCards[i].transform.localPosition = new Vector2(
                instCards[i - 1].transform.localPosition.x + cardPrefab.GetComponent<RectTransform>().sizeDelta.x + cardOffset,
				instCards[i].transform.localPosition.y
			);
			cardsPos[i] = -instCards[i].transform.localPosition;
		}
	}

    private void FixedUpdate()
	{
        if (contentRect.anchoredPosition.x >= cardsPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= cardsPos[cardsPos.Length - 1].x && !isScrolling)
		{
			scrollRect.inertia = false;
		}

		float nearestPosition = float.MaxValue;
        for (int i = 0; i < cardCount; i++)
		{
			float distance = Mathf.Abs(contentRect.anchoredPosition.x - cardsPos[i].x);

            if (distance < nearestPosition)
			{
				nearestPosition = distance;
				selectedCardId = i;
			}

			//float scale = Mathf.Clamp(1 / (distance / cardOffset) * scaleOffset, 0.5f, 1f);
			//cardsScale[i].x = Mathf.SmoothStep(instCards[i].transform.localScale.x, scale, 6 * Time.fixedDeltaTime);
			//cardsScale[i].y = Mathf.SmoothStep(instCards[i].transform.localScale.y, scale, 6 * Time.fixedDeltaTime);
			//instCards[i].transform.localScale = cardsScale[i];
		}

        for (int i = 0; i < cardCount; i++)
		{
			float distance = Mathf.Abs(contentRect.anchoredPosition.x - cardsPos[i].x);
			float scale;
			if (nearestPosition == distance)
			{
				scale = Mathf.Clamp(1f, 0.5f, 1f);
			} else
			{
				scale = Mathf.Clamp(0.5f, 0.5f, 1f);
			}

			cardsScale[i].x = Mathf.SmoothStep(instCards[i].transform.localScale.x, scale + 0.15f, 10 * Time.fixedDeltaTime);
			cardsScale[i].y = Mathf.SmoothStep(instCards[i].transform.localScale.y, scale + 0.15f, 10 * Time.fixedDeltaTime);
			instCards[i].transform.localScale = cardsScale[i];
		}

		float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling)
		{
			scrollRect.inertia = false;
		}


		if (isScrolling || scrollVelocity > 400) return;
		contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, cardsPos[selectedCardId].x, snapSpeed * Time.fixedDeltaTime);
		contentRect.anchoredPosition = contentVector;		
	}

    public void Scrolling(bool scroll)
	{
		isScrolling = scroll;
        if (scroll)
		{
			scrollRect.inertia = true;
		}
	}


}