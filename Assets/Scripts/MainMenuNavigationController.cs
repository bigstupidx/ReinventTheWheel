using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNavigationController : MonoBehaviour {
    public Image blackoutPanel;
    public float fadeSpeed;
    private AudioSource UIAudioSource;

    void Start()
    {
        
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
}
