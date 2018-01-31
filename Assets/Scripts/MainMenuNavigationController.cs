using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class MainMenuNavigationController : MonoBehaviour
{
    public Image blackoutPanel;
    public float fadeSpeed;
    private AudioSource UIAudioSource;
    public Animator cameraAnimator;
    void Start()
    {
        InitializeGooglePlay();

        UIAudioSource = GameObject.Find("UIAudioObject").GetComponent<AudioSource>();
        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }
    public void EnablePanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void DisablePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(ChangeScene(sceneName));
    }
    IEnumerator ChangeScene(string sceneName)
    {
        blackoutPanel.gameObject.SetActive(true);
        blackoutPanel.canvasRenderer.SetAlpha(0.0f);
        blackoutPanel.CrossFadeAlpha(1.0f, fadeSpeed, false);
        while(blackoutPanel.canvasRenderer.GetAlpha() < 1.0f)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
    public void MoveCameraToOptionsFromMainMenu()
    {
        cameraAnimator.SetBool("isOnOptions", true);
    }
    public void MoveCameraBackToMainMenuFromOptions()
    {
        cameraAnimator.SetBool("isOnOptions", false);
    }
    public void MoveCameraToCreditsFromMainMenu()
    {
        cameraAnimator.SetBool("isOnCredits", true);
    }
    public void MoveCameraBackToMainMenuFromCredits()
    {
        cameraAnimator.SetBool("isOnCredits", false);
    }

    public static void InitializeGooglePlay()
    {
#if UNITY_ANDROID
        //sign in to Google Play Services
        PlayGamesPlatform.Activate();

        if (!LeaderBoardController.connectedToGooglePlay)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                LeaderBoardController.connectedToGooglePlay = success;
            });
        }
#endif
    }
}
