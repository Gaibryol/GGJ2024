public partial class Constants
{
    public class GameSystem
    {
        public enum AnimalDespawnReason { OutOfTime, Success, Fail };
		public enum RecipeItems { Corn, BatWings, Beatles, Flowers, FrogLegs, Worms, Lizards, Brain, Eyeballs, Donuts, MysteryMeat, Seashells }
        public enum SprayLevel { Low, Medium, High };

        public enum Progression { Animal, Costume, Weight };

        public enum DayEndCode { Success, Fail };

        public const float SecondsPerDay = 300;

        public const int MinDayToUnlockCostume = 5;
        public const int MinDayToUnlockWeight = 10;

        public const int RequiredQuotaPerDay = 5;
	}
}