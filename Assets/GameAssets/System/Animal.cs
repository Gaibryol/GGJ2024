using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Animal : ScriptableObject
{
    public List<Constants.GameSystem.RecipeItems> recipeItems;
    public Constants.Animals.AnimalType animalType;

    public Vector2Int weightRange;
    public Vector2Int heightRange;
    public List<SprayRanges> sprayRanges;
}

[System.Serializable]
public class SprayRanges
{
    public Constants.GameSystem.SprayLevel sprayLevel;
    public Vector2Int weightRange;
    public Vector2Int heightRange;
}