using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobController : MonoBehaviour {
    private BannerView bannerView;
    // Use this for initialization
    void Start () {
#if UNITY_ANDROID
        string appId = "ca-app-pub-6737183429315610~1855552516";
#elif UNITY_IPHONE
        string appId = "ENTER APP ID";
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
