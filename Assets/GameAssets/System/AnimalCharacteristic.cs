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
    public Constants.Animals.AnimalType AnimalType;
    public Sprite Neutral;
	public Sprite Happy;
	public Sprite Sad;
}