using System.Collections.Generic;
using UnityEngine;

public class Animal : ScriptableObject
{
    public List<RecipeItems> recipeItems;
    public Constants.Animals.AnimalType animalType;
    public int weight;
    public int height;

}
