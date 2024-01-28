using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GuideBookScript : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] private Image book;
	[SerializeField] private Button prevPage;
	[SerializeField] private Button nextPage;
	[SerializeField] private Button animalsButton;
	[SerializeField] private Button identityButton;
	[SerializeField] private Button weightButton;
	[SerializeField] private Button animalsTab;
	[SerializeField] private Button identityTab;
	[SerializeField] private Button weightTab;

	[SerializeField] private Sprite lvl1Title;
	[SerializeField] private Sprite lvl2Title;
	[SerializeField] private Sprite lvl3Title;
	[SerializeField] private Sprite animalPage;
	[SerializeField] private Sprite costumePage;
	[SerializeField] private Sprite weightPage;

	private List<Sprite> pages;

	private int pageIndex;

	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void SetPageButtons()
	{
		prevPage.gameObject.SetActive(false);
		nextPage.gameObject.SetActive(false);

		if (pageIndex > 0)
		{
			prevPage.gameObject.SetActive(true);
		}
		if (pageIndex < pages.Count - 1)
		{
			nextPage.gameObject.SetActive(true);
		}
	}

	private void OnAnimals()
	{
		pageIndex = pages.IndexOf(animalPage);
		book.sprite = pages[pageIndex];
		SetPageButtons();
	}

	private void OnIdentity()
	{
		pageIndex = pages.IndexOf(costumePage);
		book.sprite = pages[pageIndex];

		SetPageButtons();
	}

	private void OnWeight()
	{
		pageIndex = pages.IndexOf(weightPage);
		book.sprite = pages[pageIndex];

		SetPageButtons();
	}

	private void UpdateBook()
	{
		pages = new List<Sprite>();
		pageIndex = 0;

		animalsButton.interactable = false;
		animalsTab.interactable = false;
		identityButton.interactable = false;
		identityTab.interactable = false;
		weightButton.interactable = false;
		weightTab.interactable = false;

		eventBroker.Publish(this, new GameSystemEvents.GetProgression((progression) => 
		{
			switch (progression)
			{
				case Constants.GameSystem.Progression.Animal:
					pages.Add(lvl1Title);
					break;
				case Constants.GameSystem.Progression.Costume:
					pages.Add(lvl2Title);
					break;
				case Constants.GameSystem.Progression.Weight:
					pages.Add(lvl3Title);
					break;
			}

			if ((int)progression >= (int)Constants.GameSystem.Progression.Animal)
			{
				pages.Add(animalPage);
				animalsButton.interactable = true;
				animalsTab.interactable = true;
			}
			if ((int)progression >= (int)Constants.GameSystem.Progression.Costume)
			{
				pages.Add(costumePage);
				identityButton.interactable = true;
				identityTab.interactable = true;
			}
			if ((int)progression >= (int)Constants.GameSystem.Progression.Weight)
			{
				pages.Add(weightPage);
				weightButton.interactable = true;
				weightTab.interactable = true;
			}
		}));

		book.sprite = pages[0];
		SetPageButtons();
	}	

	private void PreviousPage()
	{
		pageIndex = Mathf.Clamp(pageIndex - 1, 0, pages.Count - 1);
		book.sprite = pages[pageIndex];

		SetPageButtons();
	}

	private void NextPage()
	{
		pageIndex = Mathf.Clamp(pageIndex + 1, 0, pages.Count - 1);
		book.sprite = pages[pageIndex];

		SetPageButtons();
	}

	private void OnEnable()
	{
		prevPage.onClick.AddListener(PreviousPage);
		nextPage.onClick.AddListener(NextPage);

		animalsButton.onClick.AddListener(OnAnimals);
		animalsTab.onClick.AddListener(OnAnimals);
		identityButton.onClick.AddListener(OnIdentity);
		identityTab.onClick.AddListener(OnIdentity);
		weightButton.onClick.AddListener(OnWeight);
		weightTab.onClick.AddListener(OnWeight);

		UpdateBook();
	}

	private void OnDisable()
	{
		prevPage.onClick.RemoveListener(PreviousPage);
		nextPage.onClick.RemoveListener(NextPage);

		animalsButton.onClick.RemoveListener(OnAnimals);
		animalsTab.onClick.RemoveListener(OnAnimals);
		identityButton.onClick.RemoveListener(OnIdentity);
		identityTab.onClick.RemoveListener(OnIdentity);
		weightButton.onClick.RemoveListener(OnWeight);
		weightTab.onClick.RemoveListener(OnWeight);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		pageIndex = 0;
		gameObject.SetActive(false);
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BookFlip));
	}
}
