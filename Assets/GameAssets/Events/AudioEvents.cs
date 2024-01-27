using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents
{
	public class PlayMusic
	{
		public PlayMusic(string musicName, bool transition = false)
		{
			MusicName = musicName;
			Transition = transition;
		}

		public readonly string MusicName;
		public readonly bool Transition;
	}

	public class PlayTemporaryMusic
	{
		public PlayTemporaryMusic(string musicName)
		{
			MusicName = musicName;
		}

		public readonly string MusicName;
	}

	public class StopTemporaryMusic
	{
		public StopTemporaryMusic() { }
	}

	public class PlaySFX
	{
		public PlaySFX(string sfxName)
		{
			SFXName = sfxName;
		}

		public readonly string SFXName;
	}

	public class ChangeMusicVolume
	{
		public ChangeMusicVolume(float newVolume)
		{
			NewVolume = newVolume;
		}

		public readonly float NewVolume;
	}

	public class ChangeSFXVolume
	{
		public ChangeSFXVolume(float newVolume)
		{
			NewVolume = newVolume;
		}

		public readonly float NewVolume;
	}
}