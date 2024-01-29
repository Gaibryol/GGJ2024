using System;

public partial class Constants
{
    public class GameSystem
    {
        public enum AnimalDespawnReason { OutOfTime, Success, Fail, Error };
		public enum RecipeItems { Corn, BatWings, Beatles, Flowers, FrogLegs, Worms, Lizards, Brain, Eyeballs, Donuts, MysteryMeat, Seashells }
        public enum SprayLevel { Low, Medium, High };

        public enum Progression { Animal = 0, Costume = 1, Weight = 2, Patience = 3 };

        public enum DayEndCode { Success, Fail };

        public const float SecondsPerDay = 160;
		public const float SecondsPerHour = 20;

        public const int MinDayToUnlockCostume = 3;
        public const int MinDayToUnlockWeight = 5;
		public const int MinDayToUnlockPatience = 7;

		public static DateTime startingDateTime = new DateTime(2024, 1, 1, 8, 0, 0);

		public const float DelayBeforeResult = 0.5f;
		public const float DelayAfterSpray = 3f;
		public const float DelayForNextAnimal = 1.5f;

		public const int MaxSortingOrder = 99;

		// These combined is the required quota
		// Should be something else later, for now just set to 0, 0 for testing
		public const int RentCost = 5;
		public const int IngredientsCost = 5;

		public const float DialogueCycleTime = 7f;
		public const float PatienceWarningTime = 5f;

		public const string CostumeHint = "Different identities will need different ingredients in their formula! Keep that in mind!";
		public const string WeightHint = "Species vary, but so do the weights for each individual animal! Adjust the doses for each one!";
		public const string PatienceHint = "The animals are starting to get impatient, they might leave early! Time to work faster!";
	}
}