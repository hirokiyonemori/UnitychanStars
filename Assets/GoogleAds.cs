using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

#if UNITY_ANDROID
        string appId = "ca-app-pub-8148356110096114~4354473209";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
        string appId = "unexpected_platform";
#endif
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        RequestBanner();
    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8148356110096114/2083533088";

#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);

    }
}