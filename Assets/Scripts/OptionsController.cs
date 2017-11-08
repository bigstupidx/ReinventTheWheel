using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
    public AudioSource musicAudioSource;
    public AudioSource soundEffectsAudioSource;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundEffectsSlider;
    
	// Use this for initialization
	void Start () {
        //Set default values to 0.5f if this is the player's first time opening the game
        if (!PlayerPrefs.HasKey("OptionsFirstTimeSetup"))
        {
            PlayerPrefs.SetFloat(masterSlider.name, 0.5f);
            PlayerPrefs.SetFloat(musicSlider.name, 0.5f);
            PlayerPrefs.SetFloat(soundEffectsSlider.name, 0.5f);
            PlayerPrefs.SetString("OptionsFirstTimeSetup", "True");
        }
        else
        {
            masterSlider.value = PlayerPrefs.GetFloat(masterSlider.name);
            musicSlider.value = PlayerPrefs.GetFloat(musicSlider.name);
            soundEffectsSlider.value = PlayerPrefs.GetFloat(soundEffectsSlider.name);
            musicAudioSource.volume = musicSlider.value;
            soundEffectsAudioSource.volume = soundEffectsSlider.value;
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void MasterSliderVolumeChange()
    {
        AudioListener.volume = masterSlider.value;
        PlayerPrefs.SetFloat(masterSlider.name, masterSlider.value);
    }
    public void MusicSliderVolumeChange()
    {
        musicAudioSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat(musicSlider.name, musicSlider.value);
    }
    public void SoundEffectsVolumeSlider()
    {
        soundEffectsAudioSource.volume = soundEffectsSlider.value;
        PlayerPrefs.SetFloat(soundEffectsSlider.name, soundEffectsSlider.value);
    }
}
