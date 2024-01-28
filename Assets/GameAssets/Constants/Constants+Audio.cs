using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Constants
{
	public class Audio
	{
		public const string MusicVolumePP = "Music";
		public const string SFXVolumePP = "SFX";

		public const float DefaultMusicVolume = 0.25f;
		public const float DefaultSFXVolume = 0.25f;

		public const float MusicFadeSpeed = 0.25f;

		public class Music
		{
			public const string Farmyard = "Farmyard";
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
		}
	}
}