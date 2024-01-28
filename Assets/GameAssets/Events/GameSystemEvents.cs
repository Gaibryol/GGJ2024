using System.Collections.Generic;
using System;

public class GameSystemEvents
{
    public class SpawnAnimal
    {
        public readonly AnimalSpriteInfo AnimalSpriteInfo;
        public readonly int AnimalWeight;
        public readonly Constants.Animals.AnimalCostumeType AnimalCostumeType;
        public readonly Constants.Animals.AnimalDiet AnimalDiet;
        public readonly Constants.Animals.AnimalType AnimalType;
        public readonly AnimalDialogue AnimalDialogue;

        public SpawnAnimal(AnimalSpriteInfo animalSpriteInfo, int animalWeight, Constants.Animals.AnimalCostumeType animalCostumeType, 
            Constants.Animals.AnimalDiet animalDiet, Constants.Animals.AnimalType animalType, AnimalDialogue animalDialogue) 
        { 
            AnimalSpriteInfo = animalSpriteInfo;
            AnimalWeight = animalWeight;
            AnimalType = animalType;
            AnimalCostumeType = animalCostumeType;
            AnimalDiet = animalDiet;
            AnimalDialogue = animalDialogue;
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

    public class AnimalGettingImpatient
    {
        public AnimalGettingImpatient()
        {
            
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

	public class GetTotalQuota
	{
		public readonly Action<int> ProcessData;
		public GetTotalQuota(Action<int> processData)
		{
			ProcessData = processData;
		}
	}

	public class StartNextDay
	{
		public StartNextDay() { }
	}

	public class GetProgression
	{
		public readonly Action<Constants.GameSystem.Progression> ProcessData;
		public GetProgression(Action<Constants.GameSystem.Progression> processData)
		{
			ProcessData = processData;
		}
	}
}
