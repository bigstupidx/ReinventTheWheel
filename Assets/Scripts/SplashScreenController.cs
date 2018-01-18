using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {
    public string sceneNameToLoad;
    public Animation anim;

	// Use this for initialization
	void Start () {
        Invoke("LoadScene", 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
