using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Animal : ScriptableObject
{
    public List<Constants.GameSystem.RecipeItems> recipeItems;
    public Constants.Animals.AnimalType animalType;
    public Constants.Animals.AnimalDiet animalDiet;

    public Vector2Int weightRange;
    public List<SprayRanges> sprayRanges;

    public AnimalDialogue AnimalDialgues;
}

[System.Serializable]
public class SprayRanges
{
    public Constants.GameSystem.SprayLevel sprayLevel;
    public Vector2Int weightRange;
}

[System.Serializable]
public class AnimalDialogue
{
    public List<string> SuccessMessages;
    public List<string> FailureMessages;
    public List<string> PatienceMessages;
    public List<string> ChatMessages;
}