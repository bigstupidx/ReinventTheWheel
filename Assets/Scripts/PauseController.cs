using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This controller inherits from the options controller since the pause menu is pretty much the same
/// as the options menu but with some extra functionalities such as pausing
/// </summary>
public class PauseController : OptionsController {
    public GameObject pausePanel;
    public AudioSource GameStartSoundEffectsAudioSource;
    public AudioSource MammothAudioSource, chiselAudioSource;
    public bool gameIsPaused;
	// Use this for initialization
	protected override void Start () {
        //Game is default not paused
        base.Start();
        GameStartSoundEffectsAudioSource.volume = soundEffectsSlider.value;
        chiselAudioSource.volume = soundEffectsSlider.value;
        MammothAudioSource.volume = soundEffectsSlider.value;
        gameIsPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Get keyboard input for the esc key. Flip bool whenever the player presses it
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;

            //Either pause the game or unpause the game depending on the bool
            if (gameIsPaused)
                PauseGame();
            else
                UnPauseGame();

            print("here");
        }
            

	}
    //Unpause the game. Sets time scale, enables the panel, and sets bool
    public void PauseGame()
    {
        Time.timeScale = 0;
        GameStartSoundEffectsAudioSource.Pause();
        chiselAudioSource.Pause();
        MammothAudioSource.Pause();
        pausePanel.SetActive(true);
        gameIsPaused = true;
        HoleMaker.activated = false;
    }
    //Unpause the game. Resets time scale, disables the panel, and sets bool
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        GameStartSoundEffectsAudioSource.UnPause();
        chiselAudioSource.UnPause();
        MammothAudioSource.UnPause();
        pausePanel.SetActive(false);
        
        gameIsPaused = false;
        HoleMaker.activated = true;
    }
    //Load a new scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public override void SoundEffectsVolumeSlider()
    {
        base.SoundEffectsVolumeSlider();
        GameStartSoundEffectsAudioSource.volume = soundEffectsSlider.value;
        //For some reason this game object find needs to be here or else it'll throw an error
        chiselAudioSource = GameObject.Find("ChiselAudioObject").GetComponent<AudioSource>();
        chiselAudioSource.volume = soundEffectsSlider.value;
        MammothAudioSource.volume = soundEffectsSlider.value;

    }
}
