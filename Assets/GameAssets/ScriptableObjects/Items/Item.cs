using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Constants.GameSystem.RecipeItems item;

    public void SetItem(Constants.GameSystem.RecipeItems recipeItem)
    {
        item = recipeItem;
        gameObject.name = item.ToString();
    }
    public Constants.GameSystem.RecipeItems GetItem()
    {
        return item;
    }
    public void SetSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
