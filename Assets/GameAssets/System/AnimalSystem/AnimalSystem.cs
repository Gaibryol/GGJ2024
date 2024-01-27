using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSystem : MonoBehaviour
{
	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	[SerializeField] private GameObject animal;
	[SerializeField] private SpriteRenderer animalSR;

	private AnimalSpriteInfo currentSpriteInfo;

	private Vector3 animalScale;

    // Start is called before the first frame update
    void Start()
    {
		animalScale = animal.transform.localScale;
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
		// Lerp animal into view
		animalScale = new Vector3(animalScale.x, 0f, animalScale.z);
		while (animalScale.y < 1f)
		{
			animalScale = new Vector3(animalScale.x, animalScale.y + Time.deltaTime, animalScale.z);
			animal.transform.localScale = animalScale;
			yield return null;
		}

		animalScale = new Vector3(animalScale.x, 1f, animalScale.z);
		animal.transform.localScale = animalScale;
	}

	private IEnumerator SizeOut()
	{
		// Lerp animal out of view
		while (animalScale.y > 0)
		{
			animalScale = new Vector3(animalScale.x, animalScale.y - Time.deltaTime, animalScale.z);
			animal.transform.localScale = animalScale;
			yield return null;
		}

		animalScale = new Vector3(animalScale.x, 0, animalScale.z);
		animal.transform.localScale = animalScale;
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

	private void OnEnable()
	{
		eventBroker.Subscribe<GameSystemEvents.SpawnAnimal>(HandleSpawnAnimal);
		eventBroker.Subscribe<GameSystemEvents.DespawnAnimal>(HandleDespawnAnimal);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameSystemEvents.SpawnAnimal>(HandleSpawnAnimal);
		eventBroker.Unsubscribe<GameSystemEvents.DespawnAnimal>(HandleDespawnAnimal);
	}
}