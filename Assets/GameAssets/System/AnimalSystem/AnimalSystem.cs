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
		currentSpriteInfo = spriteInfo;
		animalSR.sprite = currentSpriteInfo.Neutral;
	}

	private void Success()
	{
		animalSR.sprite = currentSpriteInfo.Happy;
	}

	private void Failure()
	{
		animalSR.sprite = currentSpriteInfo.Sad;
	}

	private IEnumerator SizeIn()
	{
		float targetY = animal.transform.localScale.y;

		animalScale = new Vector3(animalScale.x, 0f, animalScale.z);
		while (animalScale.y < targetY)
		{
			animalScale = new Vector3(animalScale.x, animalScale.y + Time.deltaTime, animalScale.z);
			yield return null;
		}

		animalScale = new Vector3(animalScale.x, targetY, animalScale.z);
	}

	private IEnumerator SizeOut()
	{
		while (animalScale.y > 0)
		{
			animalScale = new Vector3(animalScale.x, animalScale.y - Time.deltaTime, animalScale.z);
			yield return null;
		}

		animalScale = new Vector3(animalScale.x, 0, animalScale.z);
	}

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		
	}
}