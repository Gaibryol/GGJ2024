using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	[SerializeField] private TMP_Text timeText;
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text weightText;

	private DateTime currentDateTime;
	private bool middleOfDay;

	private float timer;

	private int score;
	private int weight;

	private void Awake()
	{
		middleOfDay = false;
		timer = 0f;
		score = 0;
		weight = 0;
	}

	// Start is called before the first frame update
	void Start()
	{
		currentDateTime = Constants.GameSystem.startingDateTime;
		timeText.text = FormatCurrentTime();
		scoreText.text = score.ToString();
		weightText.text = weight.ToString();
	}

	// Update is called once per frame
	void Update()
    {
		if (middleOfDay)
		{
			Debug.Log(timer);
			timer += Time.deltaTime;
			if (timer > Constants.GameSystem.SecondsPerHour)
			{
				currentDateTime = currentDateTime.AddHours(1);
				timeText.text = FormatCurrentTime();
				timer = 0f;
			}
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

	public void PressSprayButton()
	{
		eventBroker.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.Medium));
	}

	private void StartDayHandler(BrokerEvent<GameSystemEvents.StartDay> inEvent)
	{
		middleOfDay = true;
		timer = 0f;
		currentDateTime = Constants.GameSystem.startingDateTime;
	}

	private void EndDayHandler(BrokerEvent<GameSystemEvents.EndDay> inEvent)
	{
		middleOfDay = false;
	}

	private void DespawnAnimalHandler(BrokerEvent<GameSystemEvents.DespawnAnimal> inEvent)
	{
		
		if (inEvent.Payload.AnimalDespawnReason == Constants.GameSystem.AnimalDespawnReason.Success)
		{
			score += 1;
			scoreText.text = score.ToString();
		}
	}

	private void SpawnAnimalHandler(BrokerEvent<GameSystemEvents.SpawnAnimal> inEvent) 
	{
		weight = inEvent.Payload.AnimalWeight;
	}
	private void OnEnable()
	{
		eventBroker.Subscribe<GameSystemEvents.StartDay>(StartDayHandler);
		eventBroker.Subscribe<GameSystemEvents.EndDay>(EndDayHandler);

		eventBroker.Subscribe<GameSystemEvents.DespawnAnimal>(DespawnAnimalHandler);
		eventBroker.Subscribe<GameSystemEvents.SpawnAnimal>(SpawnAnimalHandler);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameSystemEvents.StartDay>(StartDayHandler);
		eventBroker.Unsubscribe<GameSystemEvents.EndDay>(EndDayHandler);

		eventBroker.Unsubscribe<GameSystemEvents.DespawnAnimal>(DespawnAnimalHandler);
		eventBroker.Unsubscribe<GameSystemEvents.SpawnAnimal>(SpawnAnimalHandler);

	}
}
