using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalCostume : ScriptableObject
{
    public List<Constants.GameSystem.RecipeItems> recipeItems;

    public List<AnimalSpriteInfo> animalSprites;
}

[System.Serializable]
public class AnimalSpriteInfo
{
    public Constants.Animals.AnimalType AnimalType;
    public Sprite Neutral;
	public Sprite Happy;
	public Sprite Sad;
}