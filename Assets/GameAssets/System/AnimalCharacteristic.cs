using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalCharacteristic : ScriptableObject
{
    public List<RecipeItems> recipeItems;

    public List<AnimalSpriteInfo> animalSprites;
}

[System.Serializable]
public class AnimalSpriteInfo
{
    public Constants.Animals.AnimalType animalType;
    public Sprite Sprite;
}