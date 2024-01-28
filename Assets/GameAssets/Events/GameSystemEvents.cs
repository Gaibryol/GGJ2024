using System.Collections.Generic;

public class GameSystemEvents
{
    public class SpawnAnimal
    {
        public readonly AnimalSpriteInfo AnimalSpriteInfo;
        public readonly int AnimalWeight;
        public readonly Constants.Animals.AnimalCostumeType AnimalCostumeType;
        public readonly Constants.Animals.AnimalDiet AnimalDiet;
        public readonly Constants.Animals.AnimalType AnimalType;

        public SpawnAnimal(AnimalSpriteInfo animalSpriteInfo, int animalWeight, Constants.Animals.AnimalCostumeType animalCostumeType, 
            Constants.Animals.AnimalDiet animalDiet, Constants.Animals.AnimalType animalType) 
        { 
            AnimalSpriteInfo = animalSpriteInfo;
            AnimalWeight = animalWeight;
            AnimalType = animalType;
            AnimalCostumeType = animalCostumeType;
            AnimalDiet = animalDiet;
        }
    }

    public class DespawnAnimal
    {
        public readonly Constants.GameSystem.AnimalDespawnReason AnimalDespawnReason;

        public DespawnAnimal(Constants.GameSystem.AnimalDespawnReason animalDespawnReason)
        {
            AnimalDespawnReason = animalDespawnReason;
        }
    }


    public class AnimalSprayed
    {
        public readonly Constants.GameSystem.SprayLevel SprayLevel;

        public AnimalSprayed(Constants.GameSystem.SprayLevel sprayLevel)
        {
            SprayLevel = sprayLevel;
        }
    }

	public class ChangeAnimalSprite
	{
		public readonly Constants.GameSystem.AnimalDespawnReason AnimalDespawnReason;

		public ChangeAnimalSprite(Constants.GameSystem.AnimalDespawnReason animalDespawnReason)
		{
			AnimalDespawnReason = animalDespawnReason;
		}
	}


	public class StartDay
    {
        public readonly int Day;
        public readonly float StartTime;

        public StartDay(int day, float startTime)
        {
            Day = day;
            StartTime = startTime;
        }
    }

    public class EndDay
    {
        public readonly Constants.GameSystem.DayEndCode DayEndCode;

        public EndDay(Constants.GameSystem.DayEndCode dayEndCode)
        {
            DayEndCode = dayEndCode;
        }
    }

    public class ItemDroppedInMixer
    {
        public readonly Constants.GameSystem.RecipeItems RecipeItem;

        public ItemDroppedInMixer(Constants.GameSystem.RecipeItems recipeItem)
        {
            RecipeItem = recipeItem;
        }
    }

    public class ClearTable
    {
        public ClearTable() { }
    }

    public class ResetMixer
    {
        public ResetMixer() { }
    }
}
