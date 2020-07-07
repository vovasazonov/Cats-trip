/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

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
	private const string _nameFile = "adInfo.dat";
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
	private const string _bannerCode1 = "ca-app-pub-3940256099942544/6300978111";
    // For test InterstitialAd use "ca-app-pub-3940256099942544/1033173712"
    // For production InterstitialAd use "ca-app-pub-3742889557707024/5999222670"
    private const string _fullAdWinCode = "ca-app-pub-3940256099942544/1033173712";
    // For test video ad use "ca-app-pub-3940256099942544/5224354917"
    // For production video ad use "ca-app-pub-3742889557707024/3367322884"
    private const string _videoAdCode = "ca-app-pub-3940256099942544/5224354917";

	private InterstitialAd _fullWinAd;
	private BannerView _bannerAd;
	private RewardBasedVideoAd _videoAd;

	private AdRequest _request = new AdRequest.Builder().Build();

	// Date that helps to check amount of clicks in current date.
	private DateTime _dateToday = new DateTime();

	// Bool values of banner.
	private bool _isSentRequestBanner;
	private bool _isLoadedBanner;
	private bool _isDestroyedBanner;
	// Bool values of full ad.
	private bool _isShowedFullAd;
	private bool _isLoadedAdFull;
	private bool _isDestroyedAdFull;
	// Bool values of video ad.
	private bool _isSentRequestVideoAd;
	private bool _isLoadedVideoAd;
	private bool _isDestroyedVideoAd;

	// Ads that you want to show to user.
	private bool _fullAdWinBool;
	private bool _bannerCode1Bool;
	private bool _videoAdBool;

	// Windows in main menu
	GameObject _shopWin;
	GameObject _profileWin;
	#endregion

	#region ads

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			Instance.Data = new DataAd();

			SceneManager.activeSceneChanged += OnSceneChanged;

			InitialiseAds();
		}
	}

	private void Update()
	{
		if (Instance._fullAdWinBool)
		{
			if (RunGame.IsGameOver)
			{
				if (!Instance._isShowedFullAd)
				{
					// Show black alpha channel window if loaded and don't destroyed
					if (!Instance._isDestroyedAdFull && Instance._isLoadedAdFull)
					{
						// Show black alpha block
						GameObject.Find("Canvas").transform.Find("PanelBlack").
							gameObject.SetActive(true);

						StartCoroutine(Wait(2, () =>
						{
							//Show ad
							Instance._fullWinAd.Show();
							//Hide black alpha block
							GameObject.Find("Canvas").transform.Find("PanelBlack").
								gameObject.SetActive(false);
						}));

					}

					Instance._isShowedFullAd = true;
				}
			}
		}
		if (Instance._bannerCode1Bool)
		{
			// Load ad
			if (!Instance._isSentRequestBanner)
			{
				Instance._bannerAd.LoadAd(Instance._request);
				Instance._isSentRequestBanner = true;
			}

			if (Instance._isLoadedBanner && !Instance._isDestroyedBanner)
			{
				if (Instance._profileWin.activeInHierarchy ||
					Instance._shopWin.activeInHierarchy)
				{
					Instance._bannerAd.Show();
				}
				else
				{
					Instance._bannerAd.Hide();
				}
			}
			else
			{
				//show my banner (not admob)
			}
		}

		if (Instance._videoAdBool)
		{

		}
	}

	/// <summary>
	/// Inizialise app for ad before load ads (like banner).
	/// only once on load application.
	/// </summary>
	public void InitialiseAds()
    {
		// For ad before load ad
#if UNITY_ANDROID && !UNITY_EDITOR
        string appId = "ca-app-pub-3742889557707024~9490906439";
#elif UNITY_IPHONE && !UNITY_EDITOR
        string appId = "unexpected_platform";
#elif UNITY_EDITOR
		// Do nothing.
		string appId = "unexpected_platform";
#else
		string appId = "unexpected_platform";
        Debug.LogError("unexpected_platform");
#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);
    }

	/// <summary>
	/// Check data, and if the data are not today update values of data ad.
	/// It needs to controll fakes clicks on ad.
	/// </summary>
    public void UpdateData()
    {
        // Get the date of today
        _dateToday = DateTime.Today;

        if (_dateToday != Instance.Data.DateAd)
        {

			Instance.Data.DateAd = _dateToday;
			Instance.Data.ClicksOnAdBanner = 0;
			Instance.Data.ClicksOnAdFull = 0;

			// Save information
			LoadSave.Save(Instance.Data, NameFile);
		}
    }

	/// <summary>
	/// Load ad and set events to ads (this method do not show ad).
	/// </summary>
	private void LoadFullWinAd()
	{
		if (Instance._fullWinAd == null)
		{
			Instance._fullWinAd = new InterstitialAd(_fullAdWinCode);

			// Call when an ad is clicked.
			Instance._fullWinAd.OnAdOpening += FullWinAd_OnAdOpening;
			// Call when an ad loaded.
			Instance._fullWinAd.OnAdLoaded += FullWinAd_OnAdLoaded;
			// Call when an ad failed to load.
			Instance._fullWinAd.OnAdFailedToLoad += FullWinAd_OnAdFailedToLoad;
			// Call when ad closed.
			Instance._fullWinAd.OnAdClosed += FullWinAd_OnAdClosed;

			// Sent request and load ad
			Instance._fullWinAd.LoadAd(Instance._request);

			Instance._fullAdWinBool = true;
		}
	}

	/// <summary>
	/// Set events to ads banner.
	/// </summary>
	public void BannerAd()
	{
		if (Instance._bannerAd == null)
		{
			Instance._bannerAd = new BannerView(_bannerCode1, AdSize.Banner, AdPosition.Bottom);

			Instance._bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
			Instance._bannerAd.OnAdFailedToLoad += BannerAd_OnAdFailedToLoad;
			Instance._bannerAd.OnAdOpening += BannerAd_OnAdOpening;

			Instance._bannerCode1Bool = true;
		}
	}

	/// <summary>
	/// Load ad, set events (do not show video).
	/// </summary>
	private void VideoRewardedAd()
	{
		if (Instance._videoAd == null)
		{
			Instance._videoAd = RewardBasedVideoAd.Instance;
			Instance._videoAd.OnAdRewarded += VideoAd_OnAdRewarded;
			Instance._videoAd.OnAdLoaded += VideoAd_OnAdLoaded;
			Instance._videoAd.OnAdFailedToLoad += VideoAd_OnAdFailedToLoad;

			Instance._videoAd.LoadAd(Instance._request, _videoAdCode);
			Instance._videoAdBool = true;
		}
	}

	#region events of video rewarded ad

	/// <summary>
	/// Call when faild load video.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void VideoAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
		Instance._isDestroyedVideoAd = true;
		Instance._isLoadedVideoAd = false;

		if(SceneManager.GetActiveScene().name == "MainMenu")
		{
			GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
					Find("PanelBlack").gameObject.SetActive(false);

			//show msg that there are problems with load ad
			GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
				Find("MsgFaildLoadVideo").gameObject.SetActive(true);

			//hide msg that there are problems with load ad
			StartCoroutine(Wait(5, () => {
				GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
					Find("MsgFaildLoadVideo").gameObject.SetActive(false);
			}));
		}

		throw new NotImplementedException();
    }

	/// <summary>
	/// Call when video loaded and ready to show to user.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private void VideoAd_OnAdLoaded(object sender, EventArgs e)
    {
		Instance._isLoadedVideoAd = true;
		// Show panel black wait to ad
		if(GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
			Find("PanelBlack").gameObject.activeInHierarchy)
		{
			// Show video.
			OnButtonRewardedVideoAd();
		}
		throw new NotImplementedException();
    }

	/// <summary>
	/// Call when user finished watch video ad for get reward.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private void VideoAd_OnAdRewarded(object sender, Reward e)
    {
		if (SceneManager.GetActiveScene().name == "MainMenu")
		{
			GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
				Find("PanelBlack").gameObject.SetActive(false);

			Instance._isDestroyedVideoAd = true;

			QuestsManager.Instance.Data.WasTodayRewardVideo = true;

			LoadSave.Save(QuestsManager.Instance.Data,
				QuestsManager.Instance.NameFile);

			DataplayerManager.Instance.Data.Coins += (int)e.Amount;

			LoadSave.Save(DataplayerManager.Instance.Data);//,
				//DataplayerManager.Instance.NameFile,
				//true);

			// Update menu 
			Animals animals = new Animals();
			BackgroundMenu.SetValuesInStart(animals);
		}

		throw new NotImplementedException();
	}
	#endregion

	#region events of Interstitial ad
	/// <summary>
	/// Call when full ad closed.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void FullWinAd_OnAdClosed(object sender, EventArgs e)
    {
		// Destroy ad.
		Instance._fullWinAd.Destroy();
		Instance._isDestroyedAdFull = true;

		throw new NotImplementedException();
    }

	/// <summary>
	/// Call when user clicked on full ad.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void FullWinAd_OnAdOpening(object sender, System.EventArgs e)
    {
		//add click
		Instance.Data.ClicksOnAdFull++;

		//delete ad 
		Instance._fullWinAd.Destroy();
		Instance._isDestroyedAdFull = true;

		//save click to file
		LoadSave.Save(Instance.Data, NameFile);

        throw new System.NotImplementedException();
    }

	/// <summary>
	/// Call on full ad failed to load.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void FullWinAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
		//destroy ad
		Instance._fullWinAd.Destroy();
		Instance._isDestroyedAdFull = true;

		throw new System.NotImplementedException();

    }
	
	/// <summary>
	/// Call when full ad loaded.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void FullWinAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        // If there is less clicks than 3, show ad
        if (Instance.Data.ClicksOnAdFull > 5)
        {
			// Destroy ad
			Instance._fullWinAd.Destroy();
			Instance._isDestroyedAdFull = true;

			//show another ad or my ad

		}

        throw new System.NotImplementedException();
    }
	#endregion

	#region events of banner ad
	/// <summary>
	/// Call when banner failed to load.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void BannerAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
		Instance._isLoadedBanner = false;
		// Destroy ad
		Instance._bannerAd.Destroy();
		Instance._isDestroyedBanner = true;

		throw new NotImplementedException();
    }
	
	/// <summary>
	/// Call when user cliked on banner.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void BannerAd_OnAdOpening(object sender, EventArgs e)
    {
		// Destroy ad
		Instance._bannerAd.Destroy();
		Instance._isDestroyedBanner = true;

		Instance.Data.ClicksOnAdBanner++;

		LoadSave.Save(Instance.Data, NameFile);

		throw new NotImplementedException();
    }
	
	/// <summary>
	/// Call when banner loaded.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void BannerAd_OnAdLoaded(object sender, EventArgs e)
    {
		Instance._bannerAd.Hide();
		Instance._isLoadedBanner = true;

		if (Instance.Data.ClicksOnAdBanner >= 3)
        {
			// Destroy banner
			Instance._bannerAd.Destroy();
			Instance._isDestroyedBanner = true;
		}

        throw new NotImplementedException();
    }
	#endregion
	#endregion

	/// <summary>
	/// Call when the scene changed.
	/// </summary>
	/// <param name="current"></param>
	/// <param name="next"></param>
	private void OnSceneChanged(Scene current, Scene next)
	{
		string currentName = current.name;
		string nextName = next.name;
		
		// Destroy ads when scene are changing.
		if (Instance._fullAdWinBool)
		{
			if (!Instance._isDestroyedAdFull)
				Instance._fullWinAd.Destroy();

			Instance._fullAdWinBool = false;
		}
		if (Instance._bannerCode1Bool)
		{
			//if it is not destroyed 
			if (!Instance._isDestroyedBanner)
				Instance._bannerAd.Destroy();

			Instance._bannerCode1Bool = false;
		}
		if (Instance._videoAdBool)
		{
			Instance._videoAdBool = false;
		}

		MethodAwakeAd();

		switch (next.name)
		{
			case "GameZone":
				LoadFullWinAd();
				break;
			case "MainMenu":
				BannerAd();
				break;
			default:
				break;
		}

	}

	/// <summary>
	/// Called on start of game to set default values.
	/// </summary>
	private void MethodAwakeAd()
	{

		// Set start values in bools values
		Instance._isShowedFullAd = false;
		Instance._isSentRequestBanner = false;
		Instance._isLoadedBanner = false;
		Instance._isDestroyedBanner = false;
		Instance._isLoadedAdFull = false;
		Instance._isDestroyedAdFull = false;
		Instance._isSentRequestVideoAd = false;
		Instance._isLoadedVideoAd = false;
		Instance._isDestroyedVideoAd = false;

		Instance._bannerCode1Bool = false;
		Instance._fullAdWinBool = false;
		Instance._videoAdBool = false;

		// Get the date of today
		UpdateData();

		//get windows shop and profile (banner need)
		if (Instance._bannerCode1Bool)
		{
			Instance._shopWin = GameObject.Find("CanvasClassic").
				transform.Find("WindowShop").gameObject;
			Instance._profileWin = GameObject.Find("CanvasClassic").
				transform.Find("WindowProfile").gameObject;		
		}
	}

	/// <summary>
	/// Call when user click on button to show video on main menu.
	/// </summary>
	public void OnButtonRewardedVideoAd()
	{
		Instance._videoAdBool = true;

		if (Instance._isDestroyedVideoAd)
		{
			Instance._isSentRequestVideoAd = false;
			Instance._isDestroyedVideoAd = false;
		}

		if (!Instance._isSentRequestVideoAd)
		{
			VideoRewardedAd();
			//Instance._videoAd.LoadAd(Instance._request, _videoAdCode);
			Instance._isSentRequestVideoAd = true;
		}

		// Show panel black wait to ad
		GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
			Find("PanelBlack").gameObject.SetActive(true);

		// Show video
		if (Instance._isLoadedVideoAd && !Instance._isDestroyedVideoAd)
		{
			Instance._videoAd.Show();
		}
		
	}

	delegate void SomeMethod();
    /// <summary>
    /// Wait some time before start some method.
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    IEnumerator Wait(float seconds, SomeMethod method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }
}
