using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
public class AppodealAdvertisementsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string appKey = "6aedb1e98bc486eea15b11ad257f0afea95c59d0d5d3b36c";
        Appodeal.disableLocationPermissionCheck();
        Appodeal.initialize(appKey, Appodeal.BANNER);

        Appodeal.show(Appodeal.BANNER_BOTTOM);
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
}
