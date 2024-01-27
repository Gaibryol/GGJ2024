using System.Collections.Generic;

public class GameSystemEvents
{
    public class SpawnAnimal
    {
        public readonly AnimalSpriteInfo AnimalSpriteInfo;
        public SpawnAnimal(AnimalSpriteInfo animalSpriteInfo) 
        { 
            AnimalSpriteInfo = animalSpriteInfo;
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

        public StartDay(int day)
        {
            Day = day;
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
