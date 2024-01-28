using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	[SerializeField, Header("Main Menu")] private GameObject mainMenuScreen;
	[SerializeField] private Button startButton;

	[SerializeField, Header("Game Screen")] private TMP_Text timeText;
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text typeText;
	[SerializeField] private TMP_Text weightText;
	[SerializeField] private TMP_Text occupationText;

	[SerializeField, Header("End Screen")] private GameObject endDayScreen;
	[SerializeField] private Sprite endDaySuccess;
	[SerializeField] private Sprite endDayFail;
	[SerializeField] private TMP_Text endDayDay;
	[SerializeField] private TMP_Text endDayScore;
	[SerializeField] private TMP_Text endDayRent;
	[SerializeField] private TMP_Text endDayCosts;
	[SerializeField] private TMP_Text endDaySavings;
	[SerializeField] private TMP_Text dialogueText;
	[SerializeField] private GameObject dialogueTextScreen;
	[SerializeField] private Button nextDayButton;
	[SerializeField] private Button mainMenuButton;

	private DateTime currentDateTime;
	private bool middleOfDay;

	private float timer;
	private int score;

	private int day;

	private AnimalDialogue currentAnimalDialogues;
	private Coroutine dialogueCoroutine;

	private void Awake()
	{
		middleOfDay = false;
		timer = 0f;
		score = 0;

		day = 0;
	}

	// Start is called before the first frame update
	void Start()
	{
		currentDateTime = Constants.GameSystem.startingDateTime;
		timeText.text = FormatCurrentTime();
		scoreText.text = score.ToString();

		eventBroker.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.MainMenuTheme));
		mainMenuScreen.SetActive(true);
	}

	// Update is called once per frame
	void Update()
    {
		if (middleOfDay)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer > Constants.GameSystem.SecondsPerHour)
        {
            currentDateTime = currentDateTime.AddHours(1);
            timeText.text = FormatCurrentTime();
            timer = 0f;
        }
    }

    private string FormatCurrentTime()
	{
		if (currentDateTime.Hour < 12)
		{
			return currentDateTime.ToString("HH:mm") + " AM";
		}
		else if (currentDateTime.Hour == 12)
		{
			return currentDateTime.ToString("HH:mm") + " PM";
		}
		else
		{
			return currentDateTime.AddHours(-12).ToString("HH:mm") + " PM";
		}
	}

	private void StartNextDay()
	{
		eventBroker.Publish(this, new GameSystemEvents.StartNextDay());
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
		mainMenuScreen.SetActive(false);
	}

	private void MainMenu()
	{
		// Go to main menu
		eventBroker.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.MainMenuTheme, false));
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));

		day = 0;
	}

	private void StartDayHandler(BrokerEvent<GameSystemEvents.StartDay> inEvent)
	{
		middleOfDay = true;
		timer = 0f;
		currentDateTime = Constants.GameSystem.startingDateTime;
		timeText.text = FormatCurrentTime();
		scoreText.text = "0";
		score = 0;

		day += 1;

		endDayScreen.SetActive(false);
    }

    private void EndDayHandler(BrokerEvent<GameSystemEvents.EndDay> inEvent)
	{
		middleOfDay = false;
        SetDialogueText("");
		StopCoroutine(dialogueCoroutine);

        endDayScreen.SetActive(true);
		endDayScore.text = scoreText.text;

		if (inEvent.Payload.DayEndCode == Constants.GameSystem.DayEndCode.Success)
		{
			endDayScreen.GetComponent<Image>().sprite = endDaySuccess;

			endDayRent.text = Constants.GameSystem.RentCost.ToString();
			endDayCosts.text = Constants.GameSystem.IngredientsCost.ToString();
			endDaySavings.text = (score - Constants.GameSystem.RentCost - Constants.GameSystem.IngredientsCost).ToString();
			endDayDay.text = day.ToString();
			nextDayButton.gameObject.SetActive(true);
			mainMenuButton.gameObject.SetActive(false);
		}
		else if (inEvent.Payload.DayEndCode == Constants.GameSystem.DayEndCode.Fail)
		{
			endDayScreen.GetComponent<Image>().sprite = endDayFail;

			endDayRent.text = Constants.GameSystem.RentCost.ToString();
			endDayCosts.text = Constants.GameSystem.IngredientsCost.ToString();
			endDaySavings.text = (score - Constants.GameSystem.RentCost - Constants.GameSystem.IngredientsCost).ToString();
			endDayDay.text = day.ToString();
			mainMenuButton.gameObject.SetActive(true);
			nextDayButton.gameObject.SetActive(false);
		}
		

		if (timer < Constants.GameSystem.SecondsPerHour && timer > 9.5f)
		{
			// Should be fine, day ends always on the hour
			timer = Constants.GameSystem.SecondsPerHour;
		}
		UpdateTimer();
	}

	private void SetDialogueText(string text)
	{
		dialogueText.text = text;
		dialogueTextScreen.SetActive(text.Length > 0);
	}

    private void HandleChangeAnimalSprite(BrokerEvent<GameSystemEvents.ChangeAnimalSprite> inEvent)
    {
		StopCoroutine(dialogueCoroutine);
        if (inEvent.Payload.AnimalDespawnReason == Constants.GameSystem.AnimalDespawnReason.Success)
        {
			SetDialogueText(currentAnimalDialogues.SuccessMessages[UnityEngine.Random.Range(0, currentAnimalDialogues.SuccessMessages.Count)]);
        }
        else if (inEvent.Payload.AnimalDespawnReason == Constants.GameSystem.AnimalDespawnReason.Fail)
        {
            SetDialogueText(currentAnimalDialogues.FailureMessages[UnityEngine.Random.Range(0, currentAnimalDialogues.FailureMessages.Count)]);
        }
    }

    private void DespawnAnimalHandler(BrokerEvent<GameSystemEvents.DespawnAnimal> inEvent)
	{
		
		if (inEvent.Payload.AnimalDespawnReason == Constants.GameSystem.AnimalDespawnReason.Success)
		{
			score += 1;
			scoreText.text = score.ToString();
		}
        currentAnimalDialogues = null;
		SetDialogueText("");
    }

	private void SpawnAnimalHandler(BrokerEvent<GameSystemEvents.SpawnAnimal> inEvent) 
	{
		weightText.text = inEvent.Payload.AnimalWeight.ToString() + " kg";
		typeText.text = inEvent.Payload.AnimalDiet.ToString();

		switch (inEvent.Payload.AnimalCostumeType)
		{
			case Constants.Animals.AnimalCostumeType.Beach:
				occupationText.text = Constants.Animals.BeachIdentity;
				break;

			case Constants.Animals.AnimalCostumeType.Gang:
				occupationText.text = Constants.Animals.GangIdentity;
				break;

			case Constants.Animals.AnimalCostumeType.Policeman:
				occupationText.text = Constants.Animals.PoliceIdentity;
				break;

			case Constants.Animals.AnimalCostumeType.Student:
				occupationText.text = Constants.Animals.StudentIdentity;
				break;

			case Constants.Animals.AnimalCostumeType.Worker:
				occupationText.text = Constants.Animals.WorkerIdentity;
				break;
		}
		currentAnimalDialogues = inEvent.Payload.AnimalDialogue;
		dialogueCoroutine = StartCoroutine(CycleAnimalChatDialogue());
	}

	private IEnumerator CycleAnimalChatDialogue()
	{
		List<string> possibleDialogues = new List<string>();
		possibleDialogues.AddRange(currentAnimalDialogues.ChatMessages);
		possibleDialogues.AddRange(Constants.Animals.GenericAnimalDialogues);
		while (currentAnimalDialogues != null)
		{
			SetDialogueText(possibleDialogues[UnityEngine.Random.Range(0, possibleDialogues.Count)]);
			yield return new WaitForSeconds(Constants.GameSystem.DialogueCycleTime);
		}
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameSystemEvents.StartDay>(StartDayHandler);
		eventBroker.Subscribe<GameSystemEvents.EndDay>(EndDayHandler);

		eventBroker.Subscribe<GameSystemEvents.DespawnAnimal>(DespawnAnimalHandler);
		eventBroker.Subscribe<GameSystemEvents.SpawnAnimal>(SpawnAnimalHandler);

        eventBroker.Subscribe<GameSystemEvents.ChangeAnimalSprite>(HandleChangeAnimalSprite);

        nextDayButton.onClick.AddListener(StartNextDay);
		mainMenuButton.onClick.AddListener(MainMenu);
		startButton.onClick.AddListener(StartNextDay);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameSystemEvents.StartDay>(StartDayHandler);
		eventBroker.Unsubscribe<GameSystemEvents.EndDay>(EndDayHandler);

		eventBroker.Unsubscribe<GameSystemEvents.DespawnAnimal>(DespawnAnimalHandler);
		eventBroker.Unsubscribe<GameSystemEvents.SpawnAnimal>(SpawnAnimalHandler);

        eventBroker.Unsubscribe<GameSystemEvents.ChangeAnimalSprite>(HandleChangeAnimalSprite);

		nextDayButton.onClick.RemoveListener(StartNextDay);
		mainMenuButton.onClick.RemoveListener(MainMenu);
		startButton.onClick.RemoveListener(StartNextDay);
	}
}
