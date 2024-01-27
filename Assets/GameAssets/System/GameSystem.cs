using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
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
    #endregion

    #region Initialization Constants
    [SerializeField] private List<Animal> possibleAnimals;  // Stores specific animal recipies, weight, height, etc.
    [SerializeField] private List<Constants.Animals.AnimalType> possibleAnimalTypes;    // Linking agent between Animal, AnimalCostume, AnimalSpriteInfo
    [SerializeField] private List<AnimalCostume> possibleAnimalCostumes;  // "Costumes"
    #endregion

    #region Current Animal State
    private Animal currentAnimal;
    private Constants.Animals.AnimalType currentAnimalType;
    private AnimalCostume currentAnimalCostume;
    private int currentAnimalWeight;
    #endregion
    

    void Start()
    {
        gameProgression = Constants.GameSystem.Progression.Animal;
        StartDay();
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
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.AnimalSprayed>(OnAnimalSprayed);
        eventBrokerComponent.Unsubscribe<GameSystemEvents.ItemDroppedInMixer>(OnItemDropped);
    }

    #region Events
    private void OnItemDropped(BrokerEvent<GameSystemEvents.ItemDroppedInMixer> @event)
    {
        mixerItems.Add(@event.Payload.RecipeItem);
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

        // Get mixer items
        List<Constants.GameSystem.RecipeItems> requiredItems = CompileRecipeItems(currentAnimal, currentAnimalCostume);

        bool isCorrectSprayDuration = IsCorrectSprayDuration(@event.Payload.SprayLevel, currentAnimal);

        // Compare required items with items in mixer
        // Call either animal success or animal fail
        Constants.GameSystem.AnimalDespawnReason animalDespawnReason = IsCorrectRecipe(requiredItems, mixerItems) && isCorrectSprayDuration ? Constants.GameSystem.AnimalDespawnReason.Success : Constants.GameSystem.AnimalDespawnReason.Fail;

        // Increment the day quota if a success
        if (animalDespawnReason == Constants.GameSystem.AnimalDespawnReason.Success)
        {
            currentDayQuota++;
        }

        // clear items
        mixerItems.Clear();

        DespawnAnimal(animalDespawnReason);

        // Delay

        // Spawn in next animal
        Invoke("SpawnAnimal", 3f);
        //SpawnAnimal();

    }
    #endregion

    #region Day Logic
    private void StartDay()
    {
        dayStartTime = Time.time;

        mixerItems = new List<Constants.GameSystem.RecipeItems>();

        currentDayQuota = 0;

        eventBrokerComponent.Publish(this, new GameSystemEvents.StartDay(day, dayStartTime));

        isDayStarted = true;
        
        SpawnAnimal();

    }
    
    private void EndDay()
    {
        // End day, pause game loop
        isDayStarted = false;
        // Increment day
        day += 1;

        if (ShouldAdvanceProgress())
        {
            gameProgression++;
        }

        // Tell animal to leave
        DespawnAnimal(Constants.GameSystem.AnimalDespawnReason.OutOfTime);
        // Show progress UI

        Constants.GameSystem.DayEndCode dayEndCode = currentDayQuota >= Constants.GameSystem.RequiredQuotaPerDay ? Constants.GameSystem.DayEndCode.Success : Constants.GameSystem.DayEndCode.Fail;

        eventBrokerComponent.Publish(this, new GameSystemEvents.EndDay(dayEndCode));

    }

    private bool IsDayOver()
    {
        return Time.time - dayStartTime >= Constants.GameSystem.SecondsPerDay;
    }

    private bool ShouldAdvanceProgress()
    {
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
            Debug.LogError("An animal is already spawned...");
            return;
        }

        // Determine what animal to Spawn
        currentAnimalType = GetRandomAnimalType();
        currentAnimal = GetAnimalFromAnimalType(currentAnimalType);

        currentAnimalCostume = GetRandomAnimalCostume();
        InitializeRandomWeightHeight(currentAnimal);

        // Spawn in Animal
        // Pass in AnimalCostume to AnimalSystem
        eventBrokerComponent.Publish(this, new GameSystemEvents.SpawnAnimal(GetAnimalSpriteInfoFromAnimalType(currentAnimalType, currentAnimalCostume), currentAnimalWeight, currentAnimalHeight));
    }

    private void DespawnAnimal(Constants.GameSystem.AnimalDespawnReason animalDespawnReason)
    {
        // Destroy animal
        eventBrokerComponent.Publish(this, new GameSystemEvents.DespawnAnimal(animalDespawnReason));
        eventBrokerComponent.Publish(this, new GameSystemEvents.ClearTable());
        currentAnimal = null;
        currentAnimalCostume = null;
    }

    private AnimalCostume GetRandomAnimalCostume()
    {
        return possibleAnimalCostumes[UnityEngine.Random.Range(0, possibleAnimalCostumes.Count - 1)];
    }

    private Constants.Animals.AnimalType GetRandomAnimalType()
    {
        return possibleAnimalTypes[UnityEngine.Random.Range(0, possibleAnimalCostumes.Count - 1)];
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
        currentAnimalWeight = UnityEngine.Random.Range(animal.weightRange.x, animal.weightRange.y);
    }
    #endregion

    #region Recipe
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

        return counter.Sum() == 0;
    }


    private bool IsCorrectSprayDuration(Constants.GameSystem.SprayLevel sprayLevel, Animal animal)
    {
        // We only care if the spray duration is correct after Weight is unlocked
        if (gameProgression < Constants.GameSystem.Progression.Weight) return true;
        SprayRanges sprayRange = animal.sprayRanges.Find(range => range.sprayLevel == sprayLevel);

        if (currentAnimalWeight < sprayRange.weightRange.x || currentAnimalWeight  > sprayRange.weightRange.y) { return false; }

        return true;
    }
    #endregion
}
