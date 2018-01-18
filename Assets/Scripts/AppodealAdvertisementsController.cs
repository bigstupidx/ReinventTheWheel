using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
public class AppodealAdvertisementsController : MonoBehaviour {
    public int playThroughsUntilInterstitialAd;
    private int advertisementCounter;
	// Use this for initialization
	void Start () {
        string appKey = "6aedb1e98bc486eea15b11ad257f0afea95c59d0d5d3b36c";
        Appodeal.disableLocationPermissionCheck();
        Appodeal.initialize(appKey, Appodeal.BANNER | Appodeal.INTERSTITIAL);

        Appodeal.setTesting(true);

        //Prevent the screen from dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (PlayerPrefs.HasKey("PlayThroughs"))
            advertisementCounter = PlayerPrefs.GetInt("PlayThroughs");
        else
        {
            PlayerPrefs.SetInt("PlayThroughs", 0);
            advertisementCounter = 0;
        }
            
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Appodeal.onResume();
        }
    }

    public void HideAppodealBanner()
    {
        Appodeal.hide(Appodeal.BANNER);
    }

    public void ShowAppodealBanner()
    {
        Appodeal.show(Appodeal.BANNER_BOTTOM);
    }

    public void HideAppodealInterstitial()
    {
        Appodeal.hide(Appodeal.INTERSTITIAL);
    }

    public void ShowAppodealInterstitial()
    {
        if(PlayerPrefs.GetInt("PlayThroughs") >= playThroughsUntilInterstitialAd)
        {
            Appodeal.show(Appodeal.INTERSTITIAL);
            //reset the counter
            advertisementCounter = 0;
            PlayerPrefs.SetInt("PlayThroughs", advertisementCounter);
        }
            
    }

    public void IncrementPlayThroughCounter()
    {
        advertisementCounter++;
        PlayerPrefs.SetInt("PlayThroughs", advertisementCounter);
        Debug.Log(PlayerPrefs.GetInt("PlayThroughs"));

    }
}
