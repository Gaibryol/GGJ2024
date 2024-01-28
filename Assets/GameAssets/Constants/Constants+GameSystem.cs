using System;

public partial class Constants
{
    public class GameSystem
    {
        public enum AnimalDespawnReason { OutOfTime, Success, Fail };
		public enum RecipeItems { Corn, BatWings, Beatles, Flowers, FrogLegs, Worms, Lizards, Brain, Eyeballs, Donuts, MysteryMeat, Seashells }
        public enum SprayLevel { Low, Medium, High };

        public enum Progression { Animal, Costume, Weight, Patience };

        public enum DayEndCode { Success, Fail };

        public const float SecondsPerDay = 120;
		public const float SecondsPerHour = 10;

        public const int MinDayToUnlockCostume = 5;
        public const int MinDayToUnlockWeight = 10;
		public const int MinDayToUnlockPatience = 15;

        public const int RequiredQuotaPerDay = 5;

		public static DateTime startingDateTime = new DateTime(2024, 1, 1, 8, 0, 0);

		public const float DelayBeforeResult = 0.5f;
		public const float DelayAfterSpray = 1.5f;
		public const float DelayForNextAnimal = 2f;
	}
}