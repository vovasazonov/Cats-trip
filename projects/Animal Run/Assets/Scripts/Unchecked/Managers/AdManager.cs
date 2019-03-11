/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

/// <summary>
/// Manage with ad.
/// Give access to:
/// namefile,
/// class with data
/// </summary>
public class AdManager : MonoBehaviour {

	#region Variables
	public static AdManager Instance;

	// Data that keeping information.
	public DataAd Data { get; set; }
	// Name of file.
	private string _nameFile = "adInfo.dat";
	public string NameFile
	{
		get
		{
			return _nameFile;
		}
	}
	#endregion
	#region values of ads
	// Identify of ads admob
	// For test banner use "ca-app-pub-3940256099942544/6300978111"
	// For production banner use "ca-app-pub-3742889557707024/4531467068";
	private const string _bannerCode1 = "ca-app-pub-3742889557707024/4531467068";
    // For test InterstitialAd use "ca-app-pub-3940256099942544/1033173712"
    // For production InterstitialAd use "ca-app-pub-3742889557707024/5999222670"
    private const string _fullAdWinCode = "ca-app-pub-3742889557707024/5999222670";
    // For test video ad use "ca-app-pub-3940256099942544/5224354917"
    // For production video ad use "ca-app-pub-3742889557707024/3367322884"
    private const string _videoAdCode = "ca-app-pub-3742889557707024/3367322884";

	// Identity of test phone for show test ad
	private const string _idTestPhone = "";

	private InterstitialAd _fullWinAd;
	private BannerView _bannerAd;
	private RewardBasedVideoAd _videoAd;

	///*for testing uncomment*/
	//private AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(idTestPhoneMom).Build();
	///*for testing uncomment*/

	/*for prodoction uncomment*/
	private AdRequest _request = new AdRequest.Builder().Build();
	/*for prodoction uncomment*/

	// Date that helps to check amount of clicks in current date
	private DateTime _dateToday = new DateTime();
    #endregion

    #region ads

	/// <summary>
	/// Inizialise app for ad before load ads (like banner)
	/// only once on load application.
	/// </summary>
    public void InitialiseAds()
    {
        // For ad before load ad
#if UNITY_ANDROID && !UNITY_EDITOR
        string appId = "ca-app-pub-3742889557707024~9490906439";
        //#elif UNITY_IPHONE
        //string appId = "ca-app-pub-3940256099942544~1458002511";
#else
        string appId = "unexpected_platform";
        Debug.LogError("unexpected_platform");
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    }

	/// <summary>
	/// Update interaction with ad
	/// if day changes
	/// </summary>
    public void UpdateData()
    {
        // Get the date of today
        _dateToday = DateTime.Today;

        if (_dateToday != Data.DateAd)
        {

			Data.DateAd = _dateToday;
			Data.ClicksOnAdBanner = 0;
			Data.ClicksOnAdFull = 0;

			// Save information
			LoadSave.Save(Instance.Data, NameFile);
		}
    }

    //initialization and set events to ads

    public void FullWinAd()
    {
        if(_fullWinAd == null)
        {
            _fullWinAd = new InterstitialAd(_fullAdWinCode);

            //call when an ad is clicked
            _fullWinAd.OnAdOpening += FullWinAd_OnAdOpening;
            //call when an ad loaded
            _fullWinAd.OnAdLoaded += FullWinAd_OnAdLoaded;
            //call when an ad failed to load
            _fullWinAd.OnAdFailedToLoad += FullWinAd_OnAdFailedToLoad;
            //call when ad closed
            _fullWinAd.OnAdClosed += FullWinAd_OnAdClosed;

            //load ad
            //sent request and load ad
            _fullWinAd.LoadAd(_request);
        }
    }

    public void BannerAd()
    {
        if(_bannerAd == null)
        {
            _bannerAd = new BannerView(_bannerCode1, AdSize.Banner, AdPosition.Bottom);

            _bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
            _bannerAd.OnAdFailedToLoad += BannerAd_OnAdFailedToLoad;
            _bannerAd.OnAdOpening += BannerAd_OnAdOpening;

            _bannerAd.LoadAd(_request);
        }
    }

    public void VideoRewardedAd()
    {
        if(_videoAd == null)
        {
            _videoAd = RewardBasedVideoAd.Instance;
            _videoAd.OnAdRewarded += VideoAd_OnAdRewarded;
            _videoAd.OnAdLoaded += VideoAd_OnAdLoaded;
            _videoAd.OnAdFailedToLoad += VideoAd_OnAdFailedToLoad;

            _videoAd.LoadAd(_request, _videoAdCode);

        }
    }

    #region events of video rewarded ad

    private void VideoAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        //destroy object

        throw new NotImplementedException();
    }

    private void VideoAd_OnAdLoaded(object sender, EventArgs e)
    {

        throw new NotImplementedException();
    }
    private void VideoAd_OnAdRewarded(object sender, Reward e)
    {
        //GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
        //Find("PanelBlack").gameObject.SetActive(false);

        //isDestroyedVideoAd = true;

        ////quests data
        //DataQuests dataQuests = new DataQuests();
        //dataQuests = LoadSaveQuests.Load();

        ////player data
        //DataPlayer dataPlayer = new DataPlayer();
        //dataPlayer = LoadSavePlayer.Load();

        //dataQuests.wasTodayRewardVideo = true;
        //LoadSaveQuests.Save(dataQuests);

        //dataPlayer.coins += (int)e.Amount;
        //LoadSavePlayer.Save(dataPlayer);

        ////update menu 
        //Animals animals = new Animals();
        //BackgroundMenu.setValuesInStart(animals);

        throw new NotImplementedException();
    }
    #endregion

    #region events of Interstitial ad
    //call when full ad closed
    private void FullWinAd_OnAdClosed(object sender, EventArgs e)
    {
        //destroy ad

        throw new NotImplementedException();
    }

    //call when full ad clicked
    private void FullWinAd_OnAdOpening(object sender, System.EventArgs e)
    {
        //add click
        Data.ClicksOnAdFull++;

        //delete ad 
        _fullWinAd.Destroy();

        //save click to file
		LoadSave.Save(Data, NameFile);

        throw new System.NotImplementedException();
    }
    //call on full ad failed to load
    private void FullWinAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        //destroy ad
        _fullWinAd.Destroy();

        throw new System.NotImplementedException();

    }
    //call when full ad loaded
    private void FullWinAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        //if there is less clicks than 3, show ad
        if (Data.ClicksOnAdFull > 3)
        {
            //destroy ad
            _fullWinAd.Destroy();

            //show another ad or my ad

        }

        throw new System.NotImplementedException();
    }
    #endregion

    #region events of banner ad
    //call when banner failed to load
    private void BannerAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        //destroy ad
        _bannerAd.Destroy();

        throw new NotImplementedException();
    }
    //call on banner clicked
    private void BannerAd_OnAdOpening(object sender, EventArgs e)
    {
        //destroy ad
        _bannerAd.Destroy();

        Data.ClicksOnAdBanner++;
		IData instance;
		instance = Data;
		LoadSave.Save(Data, NameFile);

		throw new NotImplementedException();
    }
    //call when banner loaded
    private void BannerAd_OnAdLoaded(object sender, EventArgs e)
    {
        _bannerAd.Hide();

        if (Data.ClicksOnAdBanner >= 3)
        {
            //destroy banner
            _bannerAd.Destroy();
        }

        throw new NotImplementedException();
    }
	#endregion
	#endregion

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			Data = new DataAd();
		}
	}

	/// <summary>
	/// the delegate for send method like param in another method
	/// </summary>
	delegate void someMethod();
    /// <summary>
    /// wait some time before start some method
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    IEnumerator Wait(float seconds, someMethod method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }
}
