using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    #region GameState Variables
    private int day;
    private float secondsPerDay = 28800; // 3600 * hours per day
    private float dayStartTime;
    private bool isDayStarted;

    private List<RecipeItems> mixerItems;
    #endregion

    #region Initialization Constants
    [SerializeField] private List<Animal> possibleAnimals;  // Stores specific animal recipies, weight, height, etc.
    [SerializeField] private List<Constants.Animals.AnimalType> possibleAnimalTypes;    // Linking agent between Animal, AnimalCharacterstic, AnimalSpriteInfo
    [SerializeField] private List<AnimalCharacteristic> possibleAnimalCharacteristics;  // "Costumes"
    [SerializeField] private AnimalSystem animalSystem;
    #endregion

    #region Animal State
    private Animal currentAnimal;
    private Constants.Animals.AnimalType currentAnimalType;
    private AnimalCharacteristic currentAnimalCharacteristic;
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

    #region Events
    private void OnItemDropped()
    {
        //RecipeItems item;
        //mixerItems.Add(item);
    }

    private void OnAnimalSprayed()
    {
        if (animalSystem == null)
        {
            Debug.LogError("animal system doesn't exist");
            return;
        }

        if (mixerItems.Count == 0)
        {
            Debug.Log("There are no items in the mixer");
            return;
        }

        // Get mixer items
        List<RecipeItems> requiredItems = CompileRecipeItems(currentAnimal, currentAnimalCharacteristic);

        // Compare required items with items in mixer
        // Call either animal success or animal fail
        if (IsCorrectRecipe(requiredItems, mixerItems))
        {

        }
        
        // clear items
        mixerItems.Clear();

        DespawnAnimal();

        // Delay

        // Spawn in next animal
        SpawnAnimal();

    }
    #endregion

    private void StartDay()
    {
        dayStartTime = Time.time;

        if (animalSystem == null)
        {
            Debug.LogError("Animal system does not exist");
            return;
        }

        mixerItems = new List<RecipeItems>();
        SpawnAnimal();

        isDayStarted = true;
    }
    
    private void EndDay()
    {
        // End day, pause game loop
        isDayStarted = false;
        // Increment day
        day += 1;

        DespawnAnimal();
        // Tell animal to leave
        // Show progress UI
    }

    private bool IsDayOver()
    {
        return Time.time - dayStartTime >= secondsPerDay;
    }

    #region Animal
    private void SpawnAnimal()
    {
        if (animalSystem == null) 
        {
            Debug.LogError("Animal system does not exist");
            return;
        }
        // Determine what animal to Spawn
        currentAnimalType = GetRandomAnimalType();
        currentAnimalCharacteristic = GetRandomAnimalCharacteristic(currentAnimalType);

        // Spawn in Animal
        // Pass in AnimalCharacterisics to
    }

    private void DespawnAnimal()
    {
        // Destroy animal
        currentAnimalCharacteristic = null;
    }

    private AnimalCharacteristic GetRandomAnimalCharacteristic(Constants.Animals.AnimalType animalType)
    {
        System.Random random = new System.Random();

        AnimalCharacteristic selectedCharacteristic = possibleAnimalCharacteristics[random.Next(possibleAnimalCharacteristics.Count)];

        AnimalSpriteInfo spriteInfo = selectedCharacteristic.animalSprites.Find(animalSprite => animalSprite.AnimalType == animalType);

        return selectedCharacteristic;
    }

    private Constants.Animals.AnimalType GetRandomAnimalType()
    {
        System.Random random = new System.Random();

        return possibleAnimalTypes[random.Next(possibleAnimalTypes.Count)];
    }

    private Animal GetAnimalFromAnimalType(Constants.Animals animalType)
    {
        return possibleAnimals.Find(animal => animal.animalType.Equals(animalType));
    }
    #endregion

    #region Recipe
    private List<RecipeItems> CompileRecipeItems(Animal animal, AnimalCharacteristic animalCharacteristic)
    {
        List<RecipeItems> recipeItems = new List<RecipeItems>();

        recipeItems.AddRange(animal.recipeItems);
        recipeItems.AddRange(animalCharacteristic.recipeItems);

        return recipeItems;
    }


    private bool IsCorrectRecipe(List<RecipeItems> a, List<RecipeItems> b)
    {
        if (a.Count != b.Count) return false;

        int[] counter = new int[Enum.GetNames(typeof(RecipeItems)).Length];

        for (int i = 0; i < a.Count; i++)
        {
            counter[(int)a[i]]++;
            counter[(int)b[i]]--;
        }

        return counter.Sum() == 0;
    }

    #endregion
}
