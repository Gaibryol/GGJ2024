using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalCostume : ScriptableObject
{
    public List<Constants.GameSystem.RecipeItems> recipeItems;
    public Constants.Animals.AnimalCostumeType CostumeType;

    public List<AnimalSpriteInfo> animalSprites;

	public bool HasAnimalType(Constants.Animals.AnimalType type)
	{
		foreach(AnimalSpriteInfo spriteInfo in animalSprites)
		{
			if (type == spriteInfo.AnimalType)
			{
				return true;
			}
		}

		return false;
	}
}

[System.Serializable]
public class AnimalSpriteInfo
{
    public Constants.Animals.AnimalType AnimalType;
    public Sprite Neutral;
	public Sprite Happy;
	public Sprite Sad;
}