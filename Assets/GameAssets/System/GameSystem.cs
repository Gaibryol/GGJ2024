using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    #region GameState Variables
    private int day;    // current day
    private float dayStartTime; // current game time the day started at
    private bool isDayStarted;  // has the day started yet or is in waiting to start?

    private Constants.GameSystem.Progression gameProgression;

    private List<Constants.GameSystem.RecipeItems> mixerItems;

    private int currentDayQuota;
	private int totalQuota;
	private int savedQuota;
	private float currentAnimalPatience = 12f;
    #endregion

    #region Initialization Constants
    [SerializeField] private List<Animal> possibleAnimals;  // Stores specific animal recipies, weight, height, etc.
    [SerializeField] private List<Constants.Animals.AnimalType> possibleAnimalTypes;    // Linking agent between Animal, AnimalCostume, AnimalSpriteInfo
    [SerializeField] private List<AnimalCostume> possibleAnimalCostumes;  // "Costumes"

    [SerializeField] private List<Animator> smokes;

    #endregion

    #region Current Animal State
    private Animal currentAnimal;
    private Constants.Animals.AnimalType currentAnimalType;
    private AnimalCostume currentAnimalCostume;
    private int currentAnimalWeight;
	#endregion

	[SerializeField] private ParticleSystem smokeSystem;
    private List<Constants.Animals.AnimalType> randomAnimalTypesBag; // Used for fake random

    private Coroutine PatienceCoroutine;

    void Start()
    {
        gameProgression = Constants.GameSystem.Progression.Animal;
		savedQuota = 0;
		totalQuota = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDayStarted) return;

        if (IsDayOver())
        {
			EndDay();
		}
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameSystemEvents.AnimalSprayed>(OnAnimalSprayed);
        eventBrokerComponent.Subscribe<GameSystemEvents.ItemDroppedInMixer>(OnItemDropped);
        eventBrokerComponent.Subscribe<GameSystemEvents.ResetMixer>(OnResetMixer);
		eventBrokerComponent.Subscribe<GameSystemEvents.GetTotalQuota>(OnGetTotalQuota);
		eventBrokerComponent.Subscribe<GameSystemEvents.StartNextDay>(OnStartNextDay);
		eventBrokerComponent.Subscribe<GameSystemEvents.GetProgression>(OnGetProgression);
		eventBrokerComponent.Subscribe<GameSystemEvents.Restart>(OnRestart);
    }

	private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.AnimalSprayed>(OnAnimalSprayed);
        eventBrokerComponent.Unsubscribe<GameSystemEvents.ItemDroppedInMixer>(OnItemDropped);
        eventBrokerComponent.Unsubscribe<GameSystemEvents.ResetMixer>(OnResetMixer);
		eventBrokerComponent.Unsubscribe<GameSystemEvents.GetTotalQuota>(OnGetTotalQuota);
		eventBrokerComponent.Unsubscribe<GameSystemEvents.StartNextDay>(OnStartNextDay);
		eventBrokerComponent.Unsubscribe<GameSystemEvents.GetProgression>(OnGetProgression);
		eventBrokerComponent.Unsubscribe<GameSystemEvents.Restart>(OnRestart);
	}

	#region Events
	private void OnRestart(BrokerEvent<GameSystemEvents.Restart> inEvent)
	{
		StopAllCoroutines();
		gameProgression = Constants.GameSystem.Progression.Animal;
		day = 0;
		totalQuota = 0;
		savedQuota = 0;
		isDayStarted = false;
	}

	private void OnGetProgression(BrokerEvent<GameSystemEvents.GetProgression> inEvent)
	{
		inEvent.Payload.ProcessData.DynamicInvoke(gameProgression);
	}

	private void OnItemDropped(BrokerEvent<GameSystemEvents.ItemDroppedInMixer> @event)
    {
        mixerItems.Add(@event.Payload.RecipeItem);
    }

    private void OnResetMixer(BrokerEvent<GameSystemEvents.ResetMixer> @event)
    {
		if (mixerItems.Count > 0)
		{
			mixerItems.Clear();
			eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Reset));
		}
    }

    private void OnAnimalSprayed(BrokerEvent<GameSystemEvents.AnimalSprayed> @event)
    {
        if (!isDayStarted)
        {
            Debug.Log("Tried to spray when day is over");
            return;
        }

        if (currentAnimal == null)
        {
            Debug.Log("animal doesn't exist");
            return;
        }

        if (mixerItems.Count == 0)
        {
            Debug.Log("There are no items in the mixer");
            //return;
        }

		StartCoroutine(AnimalSprayed(@event.Payload.SprayLevel));
    }

	private IEnumerator AnimalSprayed(Constants.GameSystem.SprayLevel sprayLevel)
	{
		//smokeSystem.Play();
        Spray();
		eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Spray));

		if (PatienceCoroutine != null)
		{
			StopCoroutine(PatienceCoroutine);
		}
        
		// Get mixer items
		List<Constants.GameSystem.RecipeItems> requiredItems = CompileRecipeItems(currentAnimal, currentAnimalCostume);

		bool isCorrectSprayDuration = IsCorrectSprayDuration(sprayLevel, currentAnimal);

		// Compare required items with items in mixer
		// Call either animal success or animal fail
		Constants.GameSystem.AnimalDespawnReason animalDespawnReason = IsCorrectRecipe(requiredItems, mixerItems) && isCorrectSprayDuration ? Constants.GameSystem.AnimalDespawnReason.Success : Constants.GameSystem.AnimalDespawnReason.Fail;

		yield return new WaitForSeconds(Constants.GameSystem.DelayBeforeResult);

		eventBrokerComponent.Publish(this, new GameSystemEvents.ChangeAnimalSprite(animalDespawnReason));

		// Increment the day quota if a success
		if (animalDespawnReason == Constants.GameSystem.AnimalDespawnReason.Success)
		{
			currentDayQuota++;

			switch (currentAnimalType)
			{
				case Constants.Animals.AnimalType.Duck:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Duck));
					break;

				case Constants.Animals.AnimalType.Fish:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Fish));
					break;

				case Constants.Animals.AnimalType.Hippo:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Hippo));
					break;

				case Constants.Animals.AnimalType.Pig:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Pig));
					break;

				case Constants.Animals.AnimalType.Rabbit:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Rabbit));
					break;

				case Constants.Animals.AnimalType.Snake:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Snake));
					break;

				case Constants.Animals.AnimalType.Tiger:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Laughs.Tiger));
					break;
			}
		}
		else
		{
			switch (currentAnimalType)
			{
				case Constants.Animals.AnimalType.Duck:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Duck));
					break;

				case Constants.Animals.AnimalType.Fish:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Fish));
					break;

				case Constants.Animals.AnimalType.Hippo:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Hippo));
					break;

				case Constants.Animals.AnimalType.Pig:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Pig));
					break;

				case Constants.Animals.AnimalType.Rabbit:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Rabbit));
					break;

				case Constants.Animals.AnimalType.Snake:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Snake));
					break;

				case Constants.Animals.AnimalType.Tiger:
					eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.Animals.Sad.Tiger));
					break;
			}
		}
		
		yield return new WaitForSeconds(Constants.GameSystem.DelayAfterSpray);

		// clear items
		mixerItems.Clear();

		DespawnAnimal(animalDespawnReason);

		// Check if day is still going
		if (isDayStarted)
		{
			// Spawn in next animal
			Invoke("SpawnAnimal", Constants.GameSystem.DelayForNextAnimal);
		}
	}

	private void OnGetTotalQuota(BrokerEvent<GameSystemEvents.GetTotalQuota> inEvent)
	{
		inEvent.Payload.ProcessData.Invoke(totalQuota);
	}

	private void OnStartNextDay(BrokerEvent<GameSystemEvents.StartNextDay> inEvent)
	{
		StartDay();
	}
	#endregion

	#region Day Logic
	public void StartDay()
    {
        randomAnimalTypesBag = new List<Constants.Animals.AnimalType>();

        mixerItems = new List<Constants.GameSystem.RecipeItems>();

        currentDayQuota = savedQuota;

        eventBrokerComponent.Publish(this, new GameSystemEvents.StartDay(day, dayStartTime, savedQuota));
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.GameTheme, true));

        isDayStarted = true;

        dayStartTime = Time.time;
        
        SpawnAnimal();
    }
    
    private void EndDay()
    {
        // End day, pause game loop
        isDayStarted = false;
        // Increment day
        day += 1;

		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.EndDayTheme, true));

		if (ShouldAdvanceProgress())
        {
            gameProgression++;
        }

        StopAllCoroutines();
        
        // Tell animal to leave
        DespawnAnimal(Constants.GameSystem.AnimalDespawnReason.OutOfTime);

        Constants.GameSystem.DayEndCode dayEndCode = currentDayQuota >= (Constants.GameSystem.RentCost + Constants.GameSystem.IngredientsCost) ? Constants.GameSystem.DayEndCode.Success : Constants.GameSystem.DayEndCode.Fail;
		savedQuota = currentDayQuota - (Constants.GameSystem.RentCost + Constants.GameSystem.IngredientsCost);
		totalQuota += currentDayQuota;

        eventBrokerComponent.Publish(this, new GameSystemEvents.EndDay(dayEndCode, gameProgression));
    }

    private bool IsDayOver()
    {
        return Time.time - dayStartTime >= Constants.GameSystem.SecondsPerDay;
    }

    private bool ShouldAdvanceProgress()
    {
		if (day >= Constants.GameSystem.MinDayToUnlockPatience && gameProgression == Constants.GameSystem.Progression.Weight) return true;
        if (day >= Constants.GameSystem.MinDayToUnlockWeight && gameProgression == Constants.GameSystem.Progression.Costume) return true;
        if (day >= Constants.GameSystem.MinDayToUnlockCostume && gameProgression == Constants.GameSystem.Progression.Animal) return true;
        return false;
    }

    #endregion

    #region Animal
    private void SpawnAnimal()
    {
        if (currentAnimal != null)
        {
			// This will happen sometimes when an animal spawns in at the very end of the day
			// So on the next day, we despawn the old animal and spawn in a new one
            Debug.LogWarning("An animal is already spawned... spawning a new one");
			DespawnAnimal(Constants.GameSystem.AnimalDespawnReason.Error);
        }

        if (!isDayStarted)
        {
            Debug.Log("Tried to spawn when day is over.");
            return;
        }

        // Determine what animal to Spawn
        currentAnimalType = GetRandomAnimalType();
        currentAnimal = GetAnimalFromAnimalType(currentAnimalType);

        currentAnimalCostume = GetRandomAnimalCostume();
        InitializeRandomWeightHeight(currentAnimal);

		//Debug.Log("Spawning in a " + currentAnimalType + " in a " + currentAnimalCostume.name + " costume");

        // Spawn in Animal
        // Pass in AnimalCostume to AnimalSystem
        eventBrokerComponent.Publish(this, new GameSystemEvents.SpawnAnimal(GetAnimalSpriteInfoFromAnimalType(currentAnimalType, currentAnimalCostume), 
            currentAnimalWeight, currentAnimalCostume.CostumeType, currentAnimal.animalDiet, currentAnimal.animalType, currentAnimal.AnimalDialgues));

		// Check if progression has unlocked patience or gone past it
		if (gameProgression >= Constants.GameSystem.Progression.Patience)
		{
			PatienceCoroutine = StartCoroutine(AnimalPatienceHandler(currentAnimalPatience));
		}

    }

    private void DespawnAnimal(Constants.GameSystem.AnimalDespawnReason animalDespawnReason)
    {
		if (currentAnimal == null) return;

		// Destroy animal
        eventBrokerComponent.Publish(this, new GameSystemEvents.DespawnAnimal(animalDespawnReason));
        eventBrokerComponent.Publish(this, new GameSystemEvents.ClearTable());
        currentAnimal = null;
        currentAnimalCostume = null;
    }

	private IEnumerator AnimalPatienceHandler(float patienceTime)
	{
		float timer = 0f;

        bool warningSent = false;
		// Run timer while there is a current animal
		while (currentAnimal != null)
		{
			// Increment patience timer
			timer += Time.deltaTime;

            if (!warningSent && timer > patienceTime - Constants.GameSystem.PatienceWarningTime)
            {
                eventBrokerComponent.Publish(this, new GameSystemEvents.AnimalGettingImpatient());
                warningSent = true;
            }

			// If timer goes over max patience of the animal
			if (timer >= patienceTime)
			{
				// Despawn animal
				Debug.Log("Ran out of patience! Next animal");
				DespawnAnimal(Constants.GameSystem.AnimalDespawnReason.OutOfTime);
				Invoke("SpawnAnimal", Constants.GameSystem.DelayForNextAnimal);
			}
			yield return null;
		}
	}

    private AnimalCostume GetRandomAnimalCostume()
    {
		AnimalCostume costume = possibleAnimalCostumes[UnityEngine.Random.Range(0, possibleAnimalCostumes.Count - 1)];
		while (!costume.HasAnimalType(currentAnimalType))
		{
			costume = possibleAnimalCostumes[UnityEngine.Random.Range(0, possibleAnimalCostumes.Count - 1)];
		}
		return costume;
    }

    private Constants.Animals.AnimalType GetRandomAnimalType()
    {
        if (randomAnimalTypesBag.Count == 0)
        {
            randomAnimalTypesBag.AddRange(possibleAnimalTypes);
        }
        int index = UnityEngine.Random.Range(0, randomAnimalTypesBag.Count - 1);
        Constants.Animals.AnimalType selectedAnimalType = randomAnimalTypesBag[index];
        randomAnimalTypesBag.RemoveAt(index);
        return selectedAnimalType;
    }

    private AnimalSpriteInfo GetAnimalSpriteInfoFromAnimalType(Constants.Animals.AnimalType animalType, AnimalCostume animalCostume)
    {
        return animalCostume.animalSprites.Find(animalSprite => animalSprite.AnimalType == animalType);
    }

    private Animal GetAnimalFromAnimalType(Constants.Animals.AnimalType animalType)
    {
        return possibleAnimals.Find(animal => animal.animalType.Equals(animalType));
    }

    private void InitializeRandomWeightHeight(Animal animal)
    {
        if (animal == null)
        {
            Debug.LogError("tried to initialize animal but animal is null");
            return;
        }

		Array sprayLevels = Enum.GetValues(typeof(Constants.GameSystem.SprayLevel));
		Constants.GameSystem.SprayLevel sprayLevel = (Constants.GameSystem.SprayLevel)sprayLevels.GetValue(UnityEngine.Random.Range(0, sprayLevels.Length));

		foreach (SprayRanges sprayRange in animal.sprayRanges)
		{
			if (sprayRange.sprayLevel == sprayLevel)
			{
				currentAnimalWeight = UnityEngine.Random.Range(sprayRange.weightRange.x, sprayRange.weightRange.y);
			}
		}
    }
    #endregion

    #region Recipe

    private void Spray()
    {
        foreach (var anim in smokes)
        {
            anim.Play("SmokeGrow");
        }
    }
    private List<Constants.GameSystem.RecipeItems> CompileRecipeItems(Animal animal, AnimalCostume animalCostume)
    {
        List<Constants.GameSystem.RecipeItems> recipeItems = new List<Constants.GameSystem.RecipeItems>();

        recipeItems.AddRange(animal.recipeItems);

        // We only care about costumes after it is unlocked
        if (gameProgression > Constants.GameSystem.Progression.Animal)
        {
            recipeItems.AddRange(animalCostume.recipeItems);
        }

        return recipeItems;
    }


    private bool IsCorrectRecipe(List<Constants.GameSystem.RecipeItems> a, List<Constants.GameSystem.RecipeItems> b)
    {
        if (a.Count != b.Count) return false;

        int[] counter = new int[Enum.GetNames(typeof(Constants.GameSystem.RecipeItems)).Length];

        for (int i = 0; i < a.Count; i++)
        {
            counter[(int)a[i]]++;
            counter[(int)b[i]]--;
        }
        
        return !counter.Any(x => x != 0);
    }


    private bool IsCorrectSprayDuration(Constants.GameSystem.SprayLevel sprayLevel, Animal animal)
    {
        // We only care if the spray duration is correct after Weight is unlocked
        if (gameProgression < Constants.GameSystem.Progression.Weight) return true;
        SprayRanges sprayRange = animal.sprayRanges.Find(range => range.sprayLevel == sprayLevel);

        if (currentAnimalWeight < sprayRange.weightRange.x || currentAnimalWeight  > sprayRange.weightRange.y) { return false; }

        return true;
    }

    private void DebugPrintRecipe(List<Constants.GameSystem.RecipeItems> a)
    {
        string p = "";
        foreach (Constants.GameSystem.RecipeItems recipeItems in a)
        {
            p += recipeItems.ToString() + ", ";
        }
        Debug.Log(p);
    }
    #endregion
}
