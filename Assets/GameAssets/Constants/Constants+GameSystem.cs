using System;

public partial class Constants
{
    public class GameSystem
    {
        public enum AnimalDespawnReason { OutOfTime, Success, Fail, Error };
		public enum RecipeItems { Corn, BatWings, Beatles, Flowers, FrogLegs, Worms, Lizards, Brain, Eyeballs, Donuts, MysteryMeat, Seashells }
        public enum SprayLevel { Low, Medium, High };

        public enum Progression { Animal, Costume, Weight, Patience };

        public enum DayEndCode { Success, Fail };

        public const float SecondsPerDay = 12;
		public const float SecondsPerHour = 2;

        public const int MinDayToUnlockCostume = 5;
        public const int MinDayToUnlockWeight = 10;
		public const int MinDayToUnlockPatience = 15;

		public static DateTime startingDateTime = new DateTime(2024, 1, 1, 8, 0, 0);

		public const float DelayBeforeResult = 0.5f;
		public const float DelayAfterSpray = 3f;
		public const float DelayForNextAnimal = 1.5f;

		public const int MaxSortingOrder = 99;

		// These combined is the required quota
		// Should be something else later, for now just set to 0, 0 for testing
		public const int RentCost = 0;
		public const int IngredientsCost = 0;
	}
}