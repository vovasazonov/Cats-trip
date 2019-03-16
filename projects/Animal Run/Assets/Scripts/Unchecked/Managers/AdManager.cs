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
	private const string _bannerCode1 = "ca-app-pub-3940256099942544/6300978111";
    // For test InterstitialAd use "ca-app-pub-3940256099942544/1033173712"
    // For production InterstitialAd use "ca-app-pub-3742889557707024/5999222670"
    private const string _fullAdWinCode = "ca-app-pub-3940256099942544/1033173712";
    // For test video ad use "ca-app-pub-3940256099942544/5224354917"
    // For production video ad use "ca-app-pub-3742889557707024/3367322884"
    private const string _videoAdCode = "ca-app-pub-3940256099942544/5224354917";

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

	//bool values of banner
	private bool isSentRequestBanner;
	private bool _isLoadedBanner;
	private bool _isDestroyedBanner;

	//bool values of full ad
	private bool isShowedFullAd;
	private bool isLoadedAdFull;
	private bool _isDestroyedAdFull;

	//bool values of video ad
	private bool isSentRequestVideoAd;
	private bool _isLoadedVideoAd;
	private bool _isDestroyedVideoAd;
	#endregion

	#region ads

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			Data = new DataAd();

			SceneManager.activeSceneChanged += OnSceneChanged;

			InitialiseAds();

			PrepareVideoAd();
		}
	}

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
		_isDestroyedVideoAd = true;
		_isLoadedVideoAd = false;

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

		throw new NotImplementedException();
    }

	/// <summary>
	/// Call when video loaded and ready to show to user.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private void VideoAd_OnAdLoaded(object sender, EventArgs e)
    {
		_isLoadedVideoAd = true;

		throw new NotImplementedException();
    }

	/// <summary>
	/// Call when user finished watch video ad for get reward.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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

		GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
			Find("PanelBlack").gameObject.SetActive(false);

		_isDestroyedVideoAd = true;

		QuestsManager.Instance.Data.WasTodayRewardVideo = true;
		//LoadSaveQuests.Save(dataQuests);
		LoadSave.Save(QuestsManager.Instance.Data,
			QuestsManager.Instance.NameFile);

		DataplayerManager.Instance.Data.Coins += (int)e.Amount;
		//LoadSavePlayer.Save(dataPlayer);
		LoadSave.Save(DataplayerManager.Instance.Data,
			DataplayerManager.Instance.NameFile);

		//update menu 
		Animals animals = new Animals();
		BackgroundMenu.SetValuesInStart(animals);

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
		// Destrou ad.
		_fullWinAd.Destroy();
		_isDestroyedAdFull = true;

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
        Data.ClicksOnAdFull++;

        //delete ad 
        _fullWinAd.Destroy();
		_isDestroyedAdFull = true;

		//save click to file
		LoadSave.Save(Data, NameFile);

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
        _fullWinAd.Destroy();
		_isDestroyedAdFull = true;

		throw new System.NotImplementedException();

    }
	/// <summary>
	/// Call when full ad loaded.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void FullWinAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        //if there is less clicks than 3, show ad
        if (Data.ClicksOnAdFull > 3)
        {
            // Destroy ad
            _fullWinAd.Destroy();
			_isDestroyedAdFull = true;

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
		_isLoadedBanner = false;
		//destroy ad
		_bannerAd.Destroy();
		_isDestroyedBanner = true;

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
        _bannerAd.Destroy();
		_isDestroyedBanner = true;

		Data.ClicksOnAdBanner++;

		LoadSave.Save(Data, NameFile);

		throw new NotImplementedException();
    }
	// Call when banner loaded.
	private void BannerAd_OnAdLoaded(object sender, EventArgs e)
    {
        _bannerAd.Hide();
		_isLoadedBanner = true;

		if (Data.ClicksOnAdBanner >= 3)
        {
            //destroy banner
            _bannerAd.Destroy();
			_isDestroyedBanner = true;
		}

        throw new NotImplementedException();
    }
	#endregion
	#endregion

	// Ads you want to show to user.
	private bool _fullAdWinBool;
	private bool _bannerCode1Bool;
	private bool _videoAdBool;

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
		if (_fullAdWinBool)
		{
			if (!_isDestroyedAdFull)
				_fullWinAd.Destroy();

			_fullAdWinBool = false;
		}
		if (_bannerCode1Bool)
		{
			//if it is not destroyed 
			if (!_isDestroyedBanner)
				_bannerAd.Destroy();

			_bannerCode1Bool = false;
		}
		if (_videoAdBool)
		{
			_videoAdBool = false;
		}

		switch (next.name)
		{
			case "GameZone":
				PrepareFullAd();
				break;
			case "MainMenu":
				ShowBannerCode1();
				break;
			default:
				break;
		}

		MethodAwakeAd();
	}

	private void Update()
	{
		if (_fullAdWinBool)
		{
			if (RunGame.isGameOver)
			{
				if (!isShowedFullAd)
				{
					//show black alpha channel window if loaded and don't destroyed
					if (!_isDestroyedAdFull && isLoadedAdFull)
					{
						//show black alpha block
						GameObject.Find("Canvas").transform.Find("PanelBlack").gameObject.SetActive(true);

						StartCoroutine(Wait(2, () =>
						{
							//show ad
							_fullWinAd.Show();
							//hide black alpha block
							GameObject.Find("Canvas").transform.Find("PanelBlack").gameObject.SetActive(false);
						}));

					}


					isShowedFullAd = true;
				}
			}
		}
		if (_bannerCode1Bool)
		{
			//load ad
			if (!isSentRequestBanner)
			{
				_bannerAd.LoadAd(_request);
				isSentRequestBanner = true;
			}

			if (_isLoadedBanner && !_isDestroyedBanner)
			{
				if (_profileWin.activeInHierarchy || _shopWin.activeInHierarchy)
					_bannerAd.Show();
				else
					_bannerAd.Hide();
			}
			else
			{
				//show my banner (not admob)
			}
		}

		if (_videoAdBool)
		{

		}
	}

	GameObject _shopWin;
	GameObject _profileWin;

	private void MethodAwakeAd()
	{

		//set start values in bools values
		isShowedFullAd = false;
		isSentRequestBanner = false;
		_isLoadedBanner = false;
		_isDestroyedBanner = false;
		isLoadedAdFull = false;
		_isDestroyedAdFull = false;
		isSentRequestVideoAd = false;
		_isLoadedVideoAd = false;
		_isDestroyedVideoAd = false;

		//get the date of today
		_dateToday = DateTime.Today;

		if (_dateToday != AdManager.Instance.Data.DateAd)
		{
			AdManager.Instance.Data.DateAd = _dateToday;
			AdManager.Instance.Data.ClicksOnAdBanner = 0;
			AdManager.Instance.Data.ClicksOnAdFull = 0;

			//LoadSaveAd.Save(dataAd);
			LoadSave.Save(AdManager.Instance.Data, AdManager.Instance.NameFile);
		}

		//get windows shop and profile (banner need)
		if (_bannerCode1Bool)
		{
			_shopWin = GameObject.Find("CanvasClassic").transform.Find("WindowShop").gameObject;
			_profileWin = GameObject.Find("CanvasClassic").transform.Find("WindowProfile").gameObject;
		}
	}
	/// <summary>
	/// Show full ad.
	/// </summary>
	private void PrepareFullAd()
	{
		_fullWinAd = new InterstitialAd(_fullAdWinCode);

		_fullWinAd.OnAdOpening += FullWinAd_OnAdOpening;
		_fullWinAd.OnAdLoaded += FullWinAd_OnAdLoaded;
		_fullWinAd.OnAdFailedToLoad += FullWinAd_OnAdFailedToLoad;
		_fullWinAd.OnAdClosed += FullWinAd_OnAdClosed;

		//load ad
		//sent request and load ad
		_fullWinAd.LoadAd(_request);

		_fullAdWinBool = true;
	}
	/// <summary>
	/// Show banner ad.
	/// </summary>
	private void ShowBannerCode1()
	{
		_bannerAd = new BannerView(_bannerCode1, AdSize.Banner, AdPosition.Bottom);

		_bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
		_bannerAd.OnAdFailedToLoad += BannerAd_OnAdFailedToLoad;
		_bannerAd.OnAdOpening += BannerAd_OnAdOpening;

		_bannerCode1Bool = true;
	}
	/// <summary>
	/// .
	/// </summary>
	private void PrepareVideoAd()
	{
		_videoAd = RewardBasedVideoAd.Instance;
		_videoAd.OnAdRewarded += VideoAd_OnAdRewarded;
		_videoAd.OnAdLoaded += VideoAd_OnAdLoaded;
		_videoAd.OnAdFailedToLoad += VideoAd_OnAdFailedToLoad;

		_videoAdBool = true;
	}

	private void OnButtonRewardedVideoAd()
	{
		_videoAdBool = true;

		if (_isDestroyedVideoAd)
		{
			isSentRequestVideoAd = false;
			_isDestroyedVideoAd = false;
		}

		if (!isSentRequestVideoAd)
		{
			_videoAd.LoadAd(_request, _videoAdCode);
			isSentRequestVideoAd = true;
		}

		//show panel black wait to ad
		GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
			Find("PanelBlack").gameObject.SetActive(true);

		//show video
		if (_isLoadedVideoAd && !_isDestroyedVideoAd)
		{
			_videoAd.Show();
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
