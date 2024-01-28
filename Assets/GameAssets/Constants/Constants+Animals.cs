using System.Collections.Generic;

public partial class Constants
{
	public class Animals
	{
		public enum AnimalType { Pig, Duck, Hippo, Tiger, Rabbit, Snake, Fish };

		public enum AnimalDiet { Carnivore, Herbivore, Omnivore }

		public enum AnimalCostumeType { Student, Worker, Policeman, Gang, Beach }
		public const string StudentIdentity = "Student";
		public const string WorkerIdentity = "Worker";
		public const string PoliceIdentity = "Police Officer";
		public const string GangIdentity = "Gang Member";
		public const string BeachIdentity = "Beach Visitor";

		public const float lerpTime = 1.5f;

		public static readonly string[] GenericAnimalDialogues = { "Generic" };
	}
}