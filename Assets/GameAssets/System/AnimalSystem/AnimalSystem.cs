using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSystem : MonoBehaviour
{
	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	[SerializeField] private GameObject animal;
	[SerializeField] private Transform upPosition;
	[SerializeField] private Transform downPosition;

	private SpriteRenderer animalSR;

	private AnimalSpriteInfo currentSpriteInfo;

    // Start is called before the first frame update
    void Start()
    {
		animalSR = animal.GetComponent<SpriteRenderer>();
    }

	private void Spawn(AnimalSpriteInfo spriteInfo)
	{
		// Assign new animal sprite
		currentSpriteInfo = spriteInfo;
		animalSR.sprite = currentSpriteInfo.Neutral;
	}

	private void Success()
	{
		// Make animal laugh
		animalSR.sprite = currentSpriteInfo.Happy;
	}

	private void Failure()
	{
		// Make animal sad
		animalSR.sprite = currentSpriteInfo.Sad;
	}

	private IEnumerator SizeIn()
	{
		float timer = 0f;

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Swoosh));

		// Lerp animal into view
		while (timer < Constants.Animals.lerpTime)
		{
			timer += Time.deltaTime;
			animal.transform.position = Vector3.Lerp(animal.transform.position, upPosition.position, timer / Constants.Animals.lerpTime);
			yield return null;
		}

		animal.transform.position = upPosition.position;
	}

	private IEnumerator SizeOut()
	{
		float timer = 0f;

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Swoosh));

		// Lerp animal out of view
		while (timer < Constants.Animals.lerpTime)
		{
			timer += Time.deltaTime;
			animal.transform.position = Vector3.Lerp(animal.transform.position, downPosition.position, timer / Constants.Animals.lerpTime);
			yield return null;
		}

		animal.transform.position = downPosition.position;
	}

	private void HandleSpawnAnimal(BrokerEvent<GameSystemEvents.SpawnAnimal> inEvent)
	{
		Spawn(inEvent.Payload.AnimalSpriteInfo);
		StartCoroutine(SizeIn());
	}

	private void HandleDespawnAnimal(BrokerEvent<GameSystemEvents.DespawnAnimal> inEvent)
	{
		StartCoroutine(SizeOut());
	}

	private void HandleChangeAnimalSprite(BrokerEvent<GameSystemEvents.ChangeAnimalSprite> inEvent)
	{
		switch (inEvent.Payload.AnimalDespawnReason)
		{
			case Constants.GameSystem.AnimalDespawnReason.Success:
				Success();
				break;

			case Constants.GameSystem.AnimalDespawnReason.OutOfTime:
			case Constants.GameSystem.AnimalDespawnReason.Fail:
				Failure();
				break;

			case Constants.GameSystem.AnimalDespawnReason.Error:
				break;
		}
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameSystemEvents.SpawnAnimal>(HandleSpawnAnimal);
		eventBroker.Subscribe<GameSystemEvents.DespawnAnimal>(HandleDespawnAnimal);
		eventBroker.Subscribe<GameSystemEvents.ChangeAnimalSprite>(HandleChangeAnimalSprite);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameSystemEvents.SpawnAnimal>(HandleSpawnAnimal);
		eventBroker.Unsubscribe<GameSystemEvents.DespawnAnimal>(HandleDespawnAnimal);
		eventBroker.Unsubscribe<GameSystemEvents.ChangeAnimalSprite>(HandleChangeAnimalSprite);
	}
}