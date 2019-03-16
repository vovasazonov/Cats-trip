/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class AdScript : MonoBehaviour
{
    //call before deleting class
    ~AdScript()
    {
        if (fullAdWinBool)
        {
            if(!isDestroyedAdFull)
                fullWinAd.Destroy();

        }

        if (bannerCode1Bool)
        {
            //if it is not destroyed 
            if(!isDestroyedBanner)
                bannerAd.Destroy();
        }

        if (videoAdBool)
        {

        }
    }

    //in unity edior check witch ad you want to show
    [SerializeField] private bool fullAdWinBool;
    [SerializeField] private bool bannerCode1Bool;
    [SerializeField] private bool videoAdBool;

    //identify of ads admob
    //for test banner use "ca-app-pub-3940256099942544/6300978111"
    //for production banner use "ca-app-pub-3742889557707024/4531467068";
    private const string bannerCode1 = "ca-app-pub-3742889557707024/4531467068";
    //for test InterstitialAd use "ca-app-pub-3940256099942544/1033173712"
    //for production InterstitialAd use "ca-app-pub-3742889557707024/5999222670"
    private const string fullAdWinCode = "ca-app-pub-3742889557707024/5999222670";
    //for test video ad use "ca-app-pub-3940256099942544/5224354917"
    //for production video ad use "ca-app-pub-3742889557707024/3367322884"
    private const string videoAdCode = "ca-app-pub-3742889557707024/3367322884";

    private const string idTestPhoneMom = "F99D790A447C7FD3";

    InterstitialAd fullWinAd;
    BannerView bannerAd;
    RewardBasedVideoAd videoAd;

    //date that helps to check amount of clicks in current date
    DateTime dateToday = new DateTime();

    ///*for testing uncomment*/
    //private AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(idTestPhoneMom).Build();
    ///*for testing uncomment*/

    /*for prodoction uncomment*/
    private AdRequest request = new AdRequest.Builder().Build();
    /*for prodoction uncomment*/

    //bool values of banner
    private bool isSentRequestBanner;
    private bool isLoadedBanner;
    private bool isDestroyedBanner;

    //bool values of full ad
    private bool isShowedFullAd;
    private bool isLoadedAdFull;
    private bool isDestroyedAdFull;

    //bool values of video ad
    private bool isSentRequestVideoAd;
    private bool isLoadedVideoAd;
    private bool isDestroyedVideoAd;
    
    //game objects windows shop and profile
    GameObject shopWin;
    GameObject profileWin;

    private void Awake()
    {
        //set start values in bools values
        isShowedFullAd = false;
        isSentRequestBanner = false;
        isLoadedBanner = false;
        isDestroyedBanner = false;
        isLoadedAdFull = false;
        isDestroyedAdFull = false;
        isSentRequestVideoAd = false;
        isLoadedVideoAd = false;
        isDestroyedVideoAd = false;

        //get the date of today
        dateToday = DateTime.Today;

        if (dateToday != AdManager.Instance.Data.DateAd)
        {
			AdManager.Instance.Data.DateAd = dateToday;
			AdManager.Instance.Data.ClicksOnAdBanner = 0;
			AdManager.Instance.Data.ClicksOnAdFull = 0;

            //LoadSaveAd.Save(dataAd);
			LoadSave.Save(AdManager.Instance.Data, AdManager.Instance.NameFile);
        }

        //get windows shop and profile (banner need)
        if (bannerCode1Bool)
        {
            shopWin = GameObject.Find("CanvasClassic").transform.Find("WindowShop").gameObject;
            profileWin = GameObject.Find("CanvasClassic").transform.Find("WindowProfile").gameObject;
        }
    }
    
    private void Start()
    {
        //initialization and set events to ads
        if (fullAdWinBool)
        {
            fullWinAd = new InterstitialAd(fullAdWinCode);

            //call when an ad is clicked
            fullWinAd.OnAdOpening += FullWinAd_OnAdOpening;
            //call when an ad loaded
            fullWinAd.OnAdLoaded += FullWinAd_OnAdLoaded;
            //call when an ad failed to load
            fullWinAd.OnAdFailedToLoad += FullWinAd_OnAdFailedToLoad;
            //call when ad closed
            fullWinAd.OnAdClosed += FullWinAd_OnAdClosed;

            //load ad
            //sent request and load ad
            fullWinAd.LoadAd(request);

        }
        if (bannerCode1Bool)
        {
            bannerAd = new BannerView(bannerCode1, AdSize.Banner, AdPosition.Bottom);

            bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
            bannerAd.OnAdFailedToLoad += BannerAd_OnAdFailedToLoad;
            bannerAd.OnAdOpening += BannerAd_OnAdOpening;            
        }
        if (videoAdBool)
        {
            videoAd = RewardBasedVideoAd.Instance;
            videoAd.OnAdRewarded += VideoAd_OnAdRewarded;
            videoAd.OnAdLoaded += VideoAd_OnAdLoaded;
            videoAd.OnAdFailedToLoad += VideoAd_OnAdFailedToLoad;
            
        }
    }



    private void Update()
    {
        //full ad after game over
        if (fullAdWinBool)
        {
            if (RunGame.isGameOver)
            {
                if (!isShowedFullAd)
                {
                    //show black alpha channel window if loaded and don't destroyed
                    if (!isDestroyedAdFull && isLoadedAdFull)
                    {
                        //show black alpha block
                        GameObject.Find("Canvas").transform.Find("PanelBlack").gameObject.SetActive(true);

                        StartCoroutine(Wait(2, () =>
                        {
                            //show ad
                            fullWinAd.Show();
                            //hide black alpha block
                            GameObject.Find("Canvas").transform.Find("PanelBlack").gameObject.SetActive(false);
                        }));
                        
                    }


                    isShowedFullAd = true;
                }
            }
        }

        if (bannerCode1Bool)
        {
            //load ad
            if (!isSentRequestBanner)
            {
                bannerAd.LoadAd(request);
                isSentRequestBanner = true;
            }

            if (isLoadedBanner && !isDestroyedBanner)
            {
                if (profileWin.activeInHierarchy || shopWin.activeInHierarchy)
                    bannerAd.Show();
                else
                    bannerAd.Hide();
            }
            else
            {
                //show my banner (not admob)
            }
        }

        if (videoAdBool)
        {

        }
    }
    
    #region events of video rewarded ad

    private void VideoAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isDestroyedVideoAd = true;
        isLoadedVideoAd = false;
        
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
                Find("PanelBlack").gameObject.SetActive(false);

        //show msg that there are problems with load ad
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
            Find("MsgFaildLoadVideo").gameObject.SetActive(true);

        //hide msg that there are problems with load ad
        StartCoroutine(Wait(5, ()=> {
            GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
                Find("MsgFaildLoadVideo").gameObject.SetActive(false);
        }));

        throw new NotImplementedException();
    }

    private void VideoAd_OnAdLoaded(object sender, EventArgs e)
    {
        isLoadedVideoAd = true;

        throw new NotImplementedException();
    }
    private void VideoAd_OnAdRewarded(object sender, Reward e)
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
        Find("PanelBlack").gameObject.SetActive(false);

        isDestroyedVideoAd = true;

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
    //call when full ad closed
    private void FullWinAd_OnAdClosed(object sender, EventArgs e)
    {
        //delete ad 
        fullWinAd.Destroy();
        isDestroyedAdFull = true;


        throw new NotImplementedException();
    }

    //call when full ad clicked
    private void FullWinAd_OnAdOpening(object sender, System.EventArgs e)
    {
		//add click
		AdManager.Instance.Data.ClicksOnAdFull++;

        //delete ad 
        fullWinAd.Destroy();
        isDestroyedAdFull = true;

		//save click to file
		//LoadSaveAd.Save(dataAd);
		LoadSave.Save(AdManager.Instance.Data, AdManager.Instance.NameFile);

        throw new System.NotImplementedException();
    }
    //call on full ad failed to load
    private void FullWinAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        //delete ad
        fullWinAd.Destroy();
        isDestroyedAdFull = true;

        throw new System.NotImplementedException();

    }
    //call when full ad loaded
    private void FullWinAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        isLoadedAdFull = true;

        //if there is less clicks than 3, show ad
        if(AdManager.Instance.Data.ClicksOnAdFull > 3)
        {
            //delete ad
            fullWinAd.Destroy();
            isDestroyedAdFull = true;
        }

        throw new System.NotImplementedException();
    }
    #endregion

    #region events of banner ad
    //call when banner failed to load
    private void BannerAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isLoadedBanner = false;        
        bannerAd.Destroy();
        isDestroyedBanner = true;

        throw new NotImplementedException();
    }
    //call on banner clicked
    private void BannerAd_OnAdOpening(object sender, EventArgs e)
    {
        bannerAd.Destroy();
        isDestroyedBanner = true;

		AdManager.Instance.Data.ClicksOnAdBanner++;
		//LoadSaveAd.Save(dataAd);
		LoadSave.Save(AdManager.Instance.Data, AdManager.Instance.NameFile);

        throw new NotImplementedException();
    }
    //call when banner loaded
    private void BannerAd_OnAdLoaded(object sender, EventArgs e)
    {
        bannerAd.Hide();
        isLoadedBanner = true;

        if(AdManager.Instance.Data.ClicksOnAdBanner >= 3)
        {
            //destroy banner
            bannerAd.Destroy();
            isDestroyedBanner = true;
        }

        throw new NotImplementedException();
    }
    #endregion

    public void onButtonRewardedVideoAd()
    {
        if (videoAdBool)
        {
            if (isDestroyedVideoAd)
            {
                isSentRequestVideoAd = false;
                isDestroyedVideoAd = false;
            }

            if (!isSentRequestVideoAd)
            {
                videoAd.LoadAd(request, videoAdCode);
                isSentRequestVideoAd = true;
            }

            //show panel black wait to ad
            GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
                Find("PanelBlack").gameObject.SetActive(true);

            //show video
            if(isLoadedVideoAd && !isDestroyedVideoAd)
            {
                videoAd.Show();
            }
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
