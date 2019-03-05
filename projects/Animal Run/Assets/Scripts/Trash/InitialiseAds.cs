using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class InitialiseAds : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        //for ad before load ad
        #if UNITY_ANDROID
        string appId = "ca-app-pub-3742889557707024~9490906439";
        //#elif UNITY_IPHONE
        //string appId = "ca-app-pub-3940256099942544~1458002511";
        #else
        string appId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    }

}
