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

    // Start is called before the first frame update
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		pageIndex = 0;
    }

    private void PreviousPage()
	{
		pageIndex -= 1;
		if (pageIndex < 0)
		{
			pageIndex = pages.Count - 1;
		}

		book.sprite = pages[pageIndex];
	}

	private void NextPage()
	{
		pageIndex += 1;
		if (pageIndex > pages.Count - 1)
		{
			pageIndex = 0;
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
		Debug.Log("pointer down close book: " + gameObject.activeSelf);

		pageIndex = 0;
		gameObject.SetActive(false);
		this.enabled = false;
	}
}
