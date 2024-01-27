using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    #region GameState Variables
    private int day;
    private float secondsPerDay = 28800; // 3600 * hours per day
    private float dayStartTime;
    private bool isDayStarted;

    private int numberOfCharacteristicsToSelect = 2;

    private List<RecipeItems> mixerItems;
    #endregion

    #region Initialization Constants
    [SerializeField] private List<Constants.Animals.AnimalType> possibleAnimalTypes;
    [SerializeField] private List<AnimalCharacteristic> possibleAnimalCharacteristics;
    [SerializeField] private AnimalSystem animalSystem;
    #endregion

    private List<AnimalCharacteristic> activeAnimalCharacteristics;
    private Dictionary<AnimalCharacteristicType, List<AnimalCharacteristic>> sortedPossibleAnimalCharacteristics;

    // Start is called before the first frame update
    void Start()
    {
        ProcessPossibleAnimalCharacteristics();
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

    private void ProcessPossibleAnimalCharacteristics()
    {
        sortedPossibleAnimalCharacteristics = new Dictionary<AnimalCharacteristicType, List<AnimalCharacteristic>>();
        foreach (AnimalCharacteristic characteristic in possibleAnimalCharacteristics)
        {
            if (!sortedPossibleAnimalCharacteristics.ContainsKey(characteristic.characteristicType))
            {
                sortedPossibleAnimalCharacteristics.Add(characteristic.characteristicType, new List<AnimalCharacteristic>());
            }
            sortedPossibleAnimalCharacteristics[characteristic.characteristicType].Add(characteristic);
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
        List<RecipeItems> requiredItems = CompileRecipeItems(activeAnimalCharacteristics);

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
        activeAnimalCharacteristics = CompileAnimalCharacterisitcs();

        // Spawn in Animal
        // Pass in AnimalCharacterisics to
    }

    private void DespawnAnimal()
    {
        // Destroy animal
        activeAnimalCharacteristics.Clear();
    }

    private List<AnimalCharacteristic> CompileAnimalCharacterisitcs()
    {
        List<AnimalCharacteristic> characteristics = new List<AnimalCharacteristic>();

        // Get all possible animal characteristic types (hat, shirt, etc)
        List<AnimalCharacteristicType> characteristicTypes = Enum.GetValues(typeof(AnimalCharacteristicType)).Cast<AnimalCharacteristicType>().ToList();

        System.Random random = new System.Random();
        
        for (int i = 0; i < numberOfCharacteristicsToSelect; i++)
        {
            // Select a random animal characteristic type
            AnimalCharacteristicType characteristicType = (AnimalCharacteristicType)random.Next(characteristicTypes.Count);
            List<AnimalCharacteristic> possibleAnimalCharacteristic = sortedPossibleAnimalCharacteristics[characteristicType];
            AnimalCharacteristic selectedCharacteristic = possibleAnimalCharacteristic[random.Next(possibleAnimalCharacteristic.Count)];
            characteristics.Add(selectedCharacteristic);

            characteristicTypes.Remove(characteristicType);
        }


        if (characteristics.Count == 0)
        {
            Debug.Log("There are no characteristics that match");
        }

        return characteristics;
    }
    #endregion

    #region Recipe
    private List<RecipeItems> CompileRecipeItems(List<AnimalCharacteristic> animalCharacteristics)
    {
        List<RecipeItems> recipeItems = new List<RecipeItems>();

        if (animalCharacteristics.Count == 0)
        {
            Debug.LogError("This animal has no characterisitcs!");
            return recipeItems;
        }

        foreach (AnimalCharacteristic characteristic in animalCharacteristics)
        {
            recipeItems.AddRange(characteristic.recipeItems);
        }
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
