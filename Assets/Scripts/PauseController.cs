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
    public AudioSource GameStartSoundEffectsAudioSource, backgroundMusicAudioObject;
    public AudioSource MammothAudioSource, chiselAudioSource,boulderAudioObject;
    public bool gameIsPaused;
    public HoleMaker holeMakerScript;
    public GameObject retryButton;
	// Use this for initialization
	protected override void Start () {
        //Game is default not paused
        base.Start();
        GameStartSoundEffectsAudioSource.volume = soundEffectsSlider.value;
        backgroundMusicAudioObject.volume = musicSlider.value;
        chiselAudioSource.volume = soundEffectsSlider.value;
        boulderAudioObject.volume = soundEffectsSlider.value;
        MammothAudioSource.volume = soundEffectsSlider.value;
        gameIsPaused = false;
        Invoke("ActivateRetryButton", holeMakerScript.timeToChisel);
	}
	
	// Update is called once per frame
	void Update () {
        //Get keyboard input for the esc key. Flip bool whenever the player presses it
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;

            
            

            //print("here");
        }
        //Either pause the game or unpause the game depending on the bool
        if (gameIsPaused)
            PauseGame();
        else
            UnPauseGame();

    }
    //Unpause the game. Sets time scale, enables the panel, and sets bool
    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        backgroundMusicAudioObject.Pause();
        GameStartSoundEffectsAudioSource.Pause();
        chiselAudioSource.Pause();
        MammothAudioSource.Pause();
        boulderAudioObject.Pause();
        pausePanel.SetActive(true);        
        HoleMaker.activated = false;
        //pauseButton.SetActive(false);
    }
    //Unpause the game. Resets time scale, disables the panel, and sets bool
    public void UnPauseGame()
    {
        //pauseButton.SetActive(true);
        backgroundMusicAudioObject.UnPause();
        GameStartSoundEffectsAudioSource.UnPause();
        chiselAudioSource.UnPause();
        MammothAudioSource.UnPause();
        boulderAudioObject.UnPause();        
        HoleMaker.activated = true;
        pausePanel.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1;
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
        boulderAudioObject = GameObject.Find("BoulderAudioObject").GetComponent<AudioSource>();
        boulderAudioObject.volume = soundEffectsSlider.value;
        MammothAudioSource = GameObject.Find("BoulderAudioObject").GetComponent<AudioSource>();
        MammothAudioSource.volume = soundEffectsSlider.value;

    }
    public void ActivateRetryButton()
    {
        retryButton.SetActive(true);
    }
}
