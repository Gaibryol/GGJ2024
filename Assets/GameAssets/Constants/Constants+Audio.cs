using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Constants
{
	public class Audio
	{
		public const string MusicVolumePP = "Music";
		public const string SFXVolumePP = "SFX";

		public const float DefaultMusicVolume = 0.15f;
		public const float DefaultSFXVolume = 0.45f;

		public const float MusicFadeSpeed = 0.1f;

		public class Music
		{
			public const string GameTheme = "GameTheme";
			public const string MainMenuTheme = "MainMenuTheme";
			public const string EndDayTheme = "EndDayTheme";
		}

		public class SFX
		{
			public const string Drawer = "Drawer";
			public const string Spray = "Spray";
			public const string BookFlip = "Bookflip";
			public const string ButtonPress = "ButtonPress";
			public const string Mixer = "Mixer";
			public const string Reset = "Reset";
			public const string StrawSpiral = "StrawSpiral";
			public const string Straw1s = "Straw1s";
			public const string Swoosh = "Swoosh";
		}

		public class Animals
		{
			public class Laughs
			{
				public const string Duck = "DuckLaugh";
				public const string Fish = "FishLaugh";
				public const string Hippo = "HippoLaugh";
				public const string Pig = "PigLaugh";
				public const string Rabbit = "RabbitLaugh";
				public const string Snake = "SnakeLaugh";
				public const string Tiger = "TigerLaugh";
			}

			public class Sad
			{
				public const string Duck = "DuckWrong";
				public const string Fish = "FishWrong";
				public const string Hippo = "HippoWrong";
				public const string Pig = "PigWrong";
				public const string Rabbit = "RabbitWrong";
				public const string Snake = "SnakeWrong";
				public const string Tiger = "TigerWrong";
			}
		}
	}
}