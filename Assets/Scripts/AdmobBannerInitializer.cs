using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobBannerInitializer : MonoBehaviour {
    private BannerView bannerView;
    // Use this for initialization
    void Start () {
        this.RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        //Test advertisement ID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        //Production advertisement ID
        //string adUnitId = "ca-app-pub-6737183429315610/2266616059";
#elif UNITY_IPHONE
        //Test advertisement ID
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";

        //Production advertisement ID
        //string adUnitId = "ENTER ID";
#else
        string adUnitId = "unexpected_platform";
#endif
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void DestroyAd()
    {
        bannerView.Destroy();
    }
    
}
