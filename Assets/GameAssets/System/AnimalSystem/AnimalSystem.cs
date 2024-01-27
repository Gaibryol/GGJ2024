using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSystem : MonoBehaviour
{
	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	[SerializeField] private GameObject animal;
	[SerializeField] private SpriteRenderer animalSR;

	[SerializeField, Header("Animal Sprites")] private Sprite hyenaSprite;
	[SerializeField] private Sprite pigSprite;
	[SerializeField] private Sprite duckSprite;
	[SerializeField] private Sprite hippoSprite;
	[SerializeField] private Sprite cowSprite;
	[SerializeField] private Sprite rabbitSprite;
	[SerializeField] private Sprite polarBearSprite;
	[SerializeField] private Sprite chickenSprite;
	[SerializeField] private Sprite snakeSprite;
	[SerializeField] private Sprite fishSprite;

	private Vector3 animalScale;

    // Start is called before the first frame update
    void Start()
    {
		animalScale = animal.transform.localScale;
    }

	private void Spawn(Constants.Animals.AnimalType animalType, AnimalSpriteInfo spriteInfo)
	{
		switch (spriteInfo.animalType)
		{
			case Constants.Animals.AnimalType.Hyena:
				animalSR.sprite = hyenaSprite;
				break;

			case Constants.Animals.AnimalType.Pig:
				animalSR.sprite = pigSprite;
				break;

			case Constants.Animals.AnimalType.Duck:
				animalSR.sprite = duckSprite;
				break;

			case Constants.Animals.AnimalType.Hippo:
				animalSR.sprite = hippoSprite;
				break;

			case Constants.Animals.AnimalType.Cow:
				animalSR.sprite = cowSprite;
				break;

			case Constants.Animals.AnimalType.Rabbit:
				animalSR.sprite = rabbitSprite;
				break;

			case Constants.Animals.AnimalType.PolarBear:
				animalSR.sprite = polarBearSprite;
				break;

			case Constants.Animals.AnimalType.Chicken:
				animalSR.sprite = chickenSprite;
				break;

			case Constants.Animals.AnimalType.Snake:
				animalSR.sprite = snakeSprite;
				break;

			case Constants.Animals.AnimalType.Fish:
				animalSR.sprite = fishSprite;
				break;
		}
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