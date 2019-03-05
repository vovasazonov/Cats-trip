using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    public static AdManager instance;
    private DataAd data;

    #region values of ads
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

    InterstitialAd fullWinAd;
    BannerView bannerAd;
    RewardBasedVideoAd videoAd;

    //date that helps to check amount of clicks in current date
    DateTime dateToday = new DateTime();

    private AdRequest request = new AdRequest.Builder().Build();
    #endregion


    private bool isReady = false;
    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }

    public DataAd Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }
    

    public void Load()
    {
        //check if folder "saves" exists
        if (File.Exists(Application.persistentDataPath + "/saves/adInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/saves/adInfo.dat", FileMode.Open);

            //try load data
            try
            {
                //load data
                data = (DataAd)bf.Deserialize(file);
                file.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);

                //close load file
                file.Close();

                //set defoult values and save in new file
                data = new DataAd();
                data.SetDefoultData();

                //save new data
                Save();
            }
        }
        else
        {
            //set defoult values and save in new file
            data.SetDefoultData();
            Save();
        }

        isReady = true;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        //check if folder "saves" exists
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");

        FileStream file = new FileStream(Application.persistentDataPath + "/saves/adInfo.dat", FileMode.Create);

        bf.Serialize(file, data);
        file.Close();
    }

    #region ads
    public void InitialiseAds()
    {
        //for ad before load ad
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

    public void CheckDate()
    {
        //get the date of today
        dateToday = DateTime.Today;

        if (dateToday != data.dateAd)
        {
            data.dateAd = dateToday;
            data.clicksOnAdBanner = 0;
            data.clicksOnAdFull = 0;

            Save();
        }
    }

    //initialization and set events to ads

    public void FullWinAd()
    {
        if(fullWinAd == null)
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
    }

    public void BannerAd()
    {
        if(bannerAd == null)
        {
            bannerAd = new BannerView(bannerCode1, AdSize.Banner, AdPosition.Bottom);

            bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
            bannerAd.OnAdFailedToLoad += BannerAd_OnAdFailedToLoad;
            bannerAd.OnAdOpening += BannerAd_OnAdOpening;

            bannerAd.LoadAd(request);
        }
    }

    public void VideoRewardedAd()
    {
        if(videoAd == null)
        {
            videoAd = RewardBasedVideoAd.Instance;
            videoAd.OnAdRewarded += VideoAd_OnAdRewarded;
            videoAd.OnAdLoaded += VideoAd_OnAdLoaded;
            videoAd.OnAdFailedToLoad += VideoAd_OnAdFailedToLoad;

            videoAd.LoadAd(request, videoAdCode);

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
        data.clicksOnAdFull++;

        //delete ad 
        fullWinAd.Destroy();

        //save click to file
        Save();

        throw new System.NotImplementedException();
    }
    //call on full ad failed to load
    private void FullWinAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        //destroy ad
        fullWinAd.Destroy();

        throw new System.NotImplementedException();

    }
    //call when full ad loaded
    private void FullWinAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        //if there is less clicks than 3, show ad
        if (data.clicksOnAdFull > 3)
        {
            //destroy ad
            fullWinAd.Destroy();

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
        bannerAd.Destroy();

        throw new NotImplementedException();
    }
    //call on banner clicked
    private void BannerAd_OnAdOpening(object sender, EventArgs e)
    {
        //destroy ad
        bannerAd.Destroy();

        data.clicksOnAdBanner++;
        Save();

        throw new NotImplementedException();
    }
    //call when banner loaded
    private void BannerAd_OnAdLoaded(object sender, EventArgs e)
    {
        bannerAd.Hide();

        if (data.clicksOnAdBanner >= 3)
        {
            //destroy banner
            bannerAd.Destroy();
        }

        throw new NotImplementedException();
    }
    #endregion
    #endregion

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
