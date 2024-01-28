using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSystem : MonoBehaviour
{
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[SerializeField, Header("Music")] private AudioClip gameTheme;
	[SerializeField] private AudioClip mainMenuTheme;
	[SerializeField] private AudioClip endDayTheme;

	[SerializeField, Header("SFX")] private AudioClip drawer;
	[SerializeField] private AudioClip spray;
	[SerializeField] private AudioClip bookflip;
	[SerializeField] private AudioClip buttonPress;
	[SerializeField] private AudioClip mixer;
	[SerializeField] private AudioClip reset;
	[SerializeField] private AudioClip strawSpiral;
	[SerializeField] private AudioClip straw1s;
	[SerializeField] private AudioClip swoosh;

	[SerializeField] private AudioClip duckLaugh;
	[SerializeField] private AudioClip fishLaugh;
	[SerializeField] private AudioClip hippoLaugh;
	[SerializeField] private AudioClip pigLaugh;
	[SerializeField] private AudioClip rabbitLaugh;
	[SerializeField] private AudioClip snakeLaugh;
	[SerializeField] private AudioClip tigerLaugh;

	[SerializeField] private AudioClip duckWrong;
	[SerializeField] private AudioClip fishWrong;
	[SerializeField] private AudioClip hippoWrong;
	[SerializeField] private AudioClip pigWrong;
	[SerializeField] private AudioClip rabbitWrong;
	[SerializeField] private AudioClip snakeWrong;
	[SerializeField] private AudioClip tigerWrong;

	private float musicVolume;
	private float sfxVolume;

	private AudioClip oldMusic;
	private float oldTime;

	private Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
	private Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Awake()
	{
		// Set up music and sfx dictionaries
		music.Add(Constants.Audio.Music.GameTheme, gameTheme);
		music.Add(Constants.Audio.Music.MainMenuTheme, mainMenuTheme);
		music.Add(Constants.Audio.Music.EndDayTheme, endDayTheme);

		sfx.Add(Constants.Audio.SFX.Drawer, drawer);
		sfx.Add(Constants.Audio.SFX.Spray, spray);
		sfx.Add(Constants.Audio.SFX.BookFlip, bookflip);
		sfx.Add(Constants.Audio.SFX.ButtonPress, buttonPress);
		sfx.Add(Constants.Audio.SFX.Mixer, mixer);
		sfx.Add(Constants.Audio.SFX.Reset, reset);
		sfx.Add(Constants.Audio.SFX.StrawSpiral, strawSpiral);
		sfx.Add(Constants.Audio.SFX.Straw1s, straw1s);
		sfx.Add(Constants.Audio.SFX.Swoosh, swoosh);

		sfx.Add(Constants.Audio.Animals.Laughs.Duck, duckLaugh);
		sfx.Add(Constants.Audio.Animals.Laughs.Fish, fishLaugh);
		sfx.Add(Constants.Audio.Animals.Laughs.Hippo, hippoLaugh);
		sfx.Add(Constants.Audio.Animals.Laughs.Pig, pigLaugh);
		sfx.Add(Constants.Audio.Animals.Laughs.Rabbit, rabbitLaugh);
		sfx.Add(Constants.Audio.Animals.Laughs.Snake, snakeLaugh);
		sfx.Add(Constants.Audio.Animals.Laughs.Tiger, tigerLaugh);

		sfx.Add(Constants.Audio.Animals.Sad.Duck, duckWrong);
		sfx.Add(Constants.Audio.Animals.Sad.Fish, fishWrong);
		sfx.Add(Constants.Audio.Animals.Sad.Hippo, hippoWrong);
		sfx.Add(Constants.Audio.Animals.Sad.Pig, pigWrong);
		sfx.Add(Constants.Audio.Animals.Sad.Rabbit, rabbitWrong);
		sfx.Add(Constants.Audio.Animals.Sad.Snake, snakeWrong);
		sfx.Add(Constants.Audio.Animals.Sad.Tiger, tigerWrong);
	}

	private void OnEnable()
	{
		eventBrokerComponent.Subscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.PlayTemporaryMusic>(PlayTemporaryMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.StopTemporaryMusic>(StopTemporaryMusicHandler);

		float musicLevel = PlayerPrefs.GetFloat(Constants.Audio.MusicVolumePP, Constants.Audio.DefaultMusicVolume);
		float sfxLevel = PlayerPrefs.GetFloat(Constants.Audio.SFXVolumePP, Constants.Audio.DefaultSFXVolume);

		musicVolume = musicLevel;
		sfxVolume = sfxLevel;
		musicSource.volume = musicLevel;
		sfxSource.volume = sfxLevel;
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.PlayTemporaryMusic>(PlayTemporaryMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.StopTemporaryMusic>(StopTemporaryMusicHandler);
	}

	private void ChangeMusicVolumeHandler(BrokerEvent<AudioEvents.ChangeMusicVolume> inEvent)
	{
		musicVolume = inEvent.Payload.NewVolume;
		musicSource.volume = musicVolume;

		PlayerPrefs.SetFloat(Constants.Audio.MusicVolumePP, musicVolume);
	}

	private void ChangeSFXVolumeHandler(BrokerEvent<AudioEvents.ChangeSFXVolume> inEvent)
	{
		sfxVolume = inEvent.Payload.NewVolume;
		sfxSource.volume = sfxVolume;

		PlayerPrefs.SetFloat(Constants.Audio.SFXVolumePP, musicVolume);
	}

	private void PlayMusicHandler(BrokerEvent<AudioEvents.PlayMusic> inEvent)
	{
		if (inEvent.Payload.Transition)
		{
			StartCoroutine(FadeToSong(inEvent.Payload.MusicName));
		}
		else
		{
			PlayMusic(inEvent.Payload.MusicName);
		}
	}

	private void PlaySFXHandler(BrokerEvent<AudioEvents.PlaySFX> inEvent)
	{
		if (sfx.ContainsKey(inEvent.Payload.SFXName))
		{
			sfxSource.PlayOneShot(sfx[inEvent.Payload.SFXName]);
		}
		else
		{
			Debug.LogError("Cannot find sfx named " + inEvent.Payload.SFXName);
		}
	}

	private void PlayTemporaryMusicHandler(BrokerEvent<AudioEvents.PlayTemporaryMusic> inEvent)
	{
		oldMusic = musicSource.clip;
		oldTime = musicSource.time;
		StartCoroutine(FadeToSong(inEvent.Payload.MusicName));
	}

	private void StopTemporaryMusicHandler(BrokerEvent<AudioEvents.StopTemporaryMusic> inEvent)
	{
		StartCoroutine(FadeToSong(oldMusic, oldTime));
	}

	private void PlayMusic(string song, float time = 0f)
	{
		if (music.ContainsKey(song))
		{
			musicSource.Stop();
			musicSource.clip = music[song];
			musicSource.loop = true;
			musicSource.Play();
			musicSource.time = time;
		}
		else
		{
			Debug.LogError("Cannot find music named " + song);
		}
	}

	private void PlayMusic(AudioClip song, float time = 0f)
	{
		musicSource.Stop();
		musicSource.clip = song;
		musicSource.loop = true;
		musicSource.Play();
		musicSource.time = time;
	}

	private IEnumerator FadeToSong(string song, float time = 0f)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song, time);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator FadeToSong(AudioClip song, float time = 0f)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song, time);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}
}