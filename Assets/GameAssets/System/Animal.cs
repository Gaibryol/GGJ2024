using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Animal : ScriptableObject
{
    public List<Constants.GameSystem.RecipeItems> recipeItems;
    public Constants.Animals.AnimalType animalType;
    public int weight;
    public int height;

}
