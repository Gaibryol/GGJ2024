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
    private int day;
    private float secondsPerDay = 300;
    private float dayStartTime;
    private bool isDayStarted;

    private List<Constants.GameSystem.RecipeItems> mixerItems;
    #endregion

    #region Initialization Constants
    [SerializeField] private List<Animal> possibleAnimals;  // Stores specific animal recipies, weight, height, etc.
    [SerializeField] private List<Constants.Animals.AnimalType> possibleAnimalTypes;    // Linking agent between Animal, AnimalCostume, AnimalSpriteInfo
    [SerializeField] private List<AnimalCostume> possibleAnimalCostumes;  // "Costumes"
    [SerializeField] private AnimalSystem animalSystem;
    #endregion

    #region Current Animal State
    private Animal currentAnimal;
    private Constants.Animals.AnimalType currentAnimalType;
    private AnimalCostume currentAnimalCostume;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
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
        // TODO: spray time
        if (animalSystem == null)
        {
            Debug.LogError("animal system doesn't exist");
            return;
        }

        if (!isDayStarted)
        {
            Debug.Log("Tried to spray when day is over");
            return;
        }

        if (currentAnimal == null)
        {
            Debug.LogError("animal doesn't exist");
            return;
        }

        if (mixerItems.Count == 0)
        {
            Debug.Log("There are no items in the mixer");
            return;
        }

        // Get mixer items
        List<Constants.GameSystem.RecipeItems> requiredItems = CompileRecipeItems(currentAnimal, currentAnimalCostume);

        // Compare required items with items in mixer
        // Call either animal success or animal fail
        Constants.GameSystem.AnimalDespawnReason animalDespawnReason = IsCorrectRecipe(requiredItems, mixerItems) ? Constants.GameSystem.AnimalDespawnReason.Success : Constants.GameSystem.AnimalDespawnReason.Fail;

        // clear items
        mixerItems.Clear();

        DespawnAnimal(animalDespawnReason);

        // Delay

        // Spawn in next animal
        SpawnAnimal();

    }
    #endregion

    #region Day Logic
    private void StartDay()
    {
        dayStartTime = Time.time;

        if (animalSystem == null)
        {
            Debug.LogError("Animal system does not exist");
            return;
        }

        mixerItems = new List<Constants.GameSystem.RecipeItems>();

        eventBrokerComponent.Publish(this, new GameSystemEvents.StartDay(day));

        isDayStarted = true;
        
        SpawnAnimal();

    }
    
    private void EndDay()
    {
        // End day, pause game loop
        isDayStarted = false;
        // Increment day
        day += 1;

        // Tell animal to leave
        DespawnAnimal(Constants.GameSystem.AnimalDespawnReason.OutOfTime);
        // Show progress UI
        eventBrokerComponent.Publish(this, new GameSystemEvents.EndDay());

    }

    private bool IsDayOver()
    {
        return Time.time - dayStartTime >= secondsPerDay;
    }

    #endregion

    #region Animal
    private void SpawnAnimal()
    {
        if (animalSystem == null) 
        {
            Debug.LogError("Animal system does not exist");
            return;
        }

        if (currentAnimal != null)
        {
            Debug.LogError("An animal is already spawned...");
            return;
        }

        // Determine what animal to Spawn
        currentAnimalType = GetRandomAnimalType();
        currentAnimal = GetAnimalFromAnimalType(currentAnimalType);
        currentAnimalCostume = GetRandomAnimalCostume(currentAnimalType);

        // Spawn in Animal
        // Pass in AnimalCostume to AnimalSystem
        eventBrokerComponent.Publish(this, new GameSystemEvents.SpawnAnimal(GetAnimalSpriteInfoFromAnimalType(currentAnimalType, currentAnimalCostume)));
    }

    private void DespawnAnimal(Constants.GameSystem.AnimalDespawnReason animalDespawnReason)
    {
        // Destroy animal
        eventBrokerComponent.Publish(this, new GameSystemEvents.DespawnAnimal(animalDespawnReason));
        currentAnimal = null;
        currentAnimalCostume = null;
    }

    private AnimalCostume GetRandomAnimalCostume(Constants.Animals.AnimalType animalType)
    {
        System.Random random = new System.Random();

        AnimalCostume selectedCostume = possibleAnimalCostumes[random.Next(possibleAnimalCostumes.Count)];


        return selectedCostume;
    }

    private Constants.Animals.AnimalType GetRandomAnimalType()
    {
        System.Random random = new System.Random();

        return possibleAnimalTypes[random.Next(possibleAnimalTypes.Count)];
    }

    private AnimalSpriteInfo GetAnimalSpriteInfoFromAnimalType(Constants.Animals.AnimalType animalType, AnimalCostume animalCostume)
    {
        return animalCostume.animalSprites.Find(animalSprite => animalSprite.AnimalType == animalType);
    }

    private Animal GetAnimalFromAnimalType(Constants.Animals.AnimalType animalType)
    {
        return possibleAnimals.Find(animal => animal.animalType.Equals(animalType));
    }
    #endregion

    #region Recipe
    private List<Constants.GameSystem.RecipeItems> CompileRecipeItems(Animal animal, AnimalCostume animalCostume)
    {
        List<Constants.GameSystem.RecipeItems> recipeItems = new List<Constants.GameSystem.RecipeItems>();

        recipeItems.AddRange(animal.recipeItems);
        recipeItems.AddRange(animalCostume.recipeItems);

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

    #endregion
}
