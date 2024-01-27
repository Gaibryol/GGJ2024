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
        public readonly float Duration;

        public AnimalSprayed(float duration)
        {
            Duration = duration;
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
        public readonly RecipeItems RecipeItem;

        public ItemDroppedInMixer(RecipeItems recipeItem)
        {
            RecipeItem = recipeItem;
        }
    }
}
