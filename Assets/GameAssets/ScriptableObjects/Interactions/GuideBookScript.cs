using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GuideBookScript : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] private Image book;
	[SerializeField] private List<Sprite> pages;
	[SerializeField] private Button prevPage;
	[SerializeField] private Button nextPage;

	private SpriteRenderer spriteRenderer;

	private int pageIndex;

	private EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		pageIndex = 0;
    }

    private void PreviousPage()
	{
		if (pageIndex > 0)
		{
			pageIndex -= 1;
			nextPage.gameObject.SetActive(true);
		}
		else
		{
			pageIndex = 0;
			prevPage.gameObject.SetActive(false);
		}

		book.sprite = pages[pageIndex];
	}

	private void NextPage()
	{
		if (pageIndex < pages.Count - 1)
		{
			pageIndex += 1;
			prevPage.gameObject.SetActive(true);
		}
		else
		{
			pageIndex = pages.Count - 1;
			nextPage.gameObject.SetActive(false);
		}

		book.sprite = pages[pageIndex];
	}

	private void OnEnable()
	{
		prevPage.onClick.AddListener(PreviousPage);
		nextPage.onClick.AddListener(NextPage);
	}

	private void OnDisable()
	{
		prevPage.onClick.RemoveListener(PreviousPage);
		nextPage.onClick.RemoveListener(NextPage);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		pageIndex = 0;
		gameObject.SetActive(false);
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BookFlip));
	}
}
