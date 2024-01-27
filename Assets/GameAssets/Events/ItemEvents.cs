using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEvents
{
    public List<Constants.GameSystem.RecipeItems> Ingredients { get; private set; }

    public ItemEvents(List<Constants.GameSystem.RecipeItems> ingredients)
    {
        Ingredients = ingredients;
    }
}
