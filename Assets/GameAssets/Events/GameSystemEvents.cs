using System.Collections.Generic;

public class GameSystemEvents
{
    public class SpawnAnimal
    {
        public readonly AnimalSpriteInfo AnimalSpriteInfo;
        public readonly int AnimalWeight;
        public readonly int AnimalHeight;
        public SpawnAnimal(AnimalSpriteInfo animalSpriteInfo, int animalWeight, int animalHeight) 
        { 
            AnimalSpriteInfo = animalSpriteInfo;
            AnimalWeight = animalWeight;
            AnimalHeight = animalHeight;
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
        public EndDay()
        {
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
}
