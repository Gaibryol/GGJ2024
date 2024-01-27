public partial class Constants
{
    public class GameSystem
    {
        public enum AnimalDespawnReason { OutOfTime, Success, Fail };
		public enum RecipeItems { itemA, itemB, itemC }
        public enum SprayLevel { Low, Medium, High };

        public enum Progression { Animal, Costume, Weight };


        public const float SecondsPerDay = 300;

        public const int MinDayToUnlockCostume = 5;
        public const int MinDayToUnlockWeight = 10;
	}
}