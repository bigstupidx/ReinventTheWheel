using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Loads a scene. 
/// Called in an animation event in the Monument Games Splash Screen scene
/// </summary>
public class SplashScreenSceneLoader : MonoBehaviour {
    
    //Loads the scene with the passed string
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
