using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class ButtonClick : MonoBehaviour {

    ScreenFader screenFader;

    public GameObject sectionAnimalsInShop;
    private Animals animals = new Animals();   

    public void ClickLaterButton()
    {
        DataAd data = LoadSaveAd.Load();
        data.clicksOnRestartButton = 0;
        LoadSaveAd.Save(data);

        //hide rate window
        GameObject.Find("CanvasClassic").transform.
            Find("WindowRate").gameObject.SetActive(false);
    }

    public void ClickRateButton()
    {
        DataAd data = LoadSaveAd.Load();
        data.clicksOnRestartButton = 0;
        data.isRatedApp = true;
        LoadSaveAd.Save(data);

        //hide rate window
        GameObject.Find("CanvasClassic").transform.
            Find("WindowRate").gameObject.SetActive(false);
        //open play market to rate my app
        Application.OpenURL("market://details?id=com.Nroma.Catstrip");
    }

    /// <summary>
    /// run game (for menu when click play button)
    /// </summary>
    public void ClickPlayButton()
    {
        screenFader = GetComponent<ScreenFader>();      
        StartCoroutine(WaitTimeGoToSceneGame(screenFader));
        
    }

    public void ClickShopButton()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").gameObject.SetActive(true);
    }

    public void ClickProfileButton()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowProfile").gameObject.SetActive(true);
    }

    public void ClickQuestsButton()
    {
        DataQuests dataQuests = new DataQuests();
        dataQuests = LoadSaveQuests.Load();

        //get button video rewarded
        GameObject buttonRewardVideo = GameObject.Find("CanvasClassic").transform.
            Find("WindowQuests").Find("ButtonRewardVideo").gameObject;

        if (dataQuests.dateQuest != DateTime.Today)
        {
            dataQuests.SetDefoultData();
            LoadSaveQuests.Save(dataQuests);
        }

        if(dataQuests.wasTodayRewardVideo)
        {
            buttonRewardVideo.SetActive(false);

            //show text that there are not rewarded
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("NoGiftsText").gameObject.SetActive(true);
        }
        else
        {
            buttonRewardVideo.SetActive(true);
        }

        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").gameObject.SetActive(true);
    }

    public void ClickMusicButton(GameObject buttonMusic)
    {
        Animals animals = new Animals();
        DataPlayer dataPlayer = new DataPlayer();
        dataPlayer = LoadSavePlayer.Load();

        if(buttonMusic.GetComponent<Toggle>().isOn == true)
        {
            dataPlayer.isMusicMainMenu = true;
        }
        else
        {
            dataPlayer.isMusicMainMenu = false;
        }

        LoadSavePlayer.Save(dataPlayer);

        BackgroundMenu.setValuesInStart(animals);
    }

    //coroutin that make effect of shadow and after this start scene GameZone
    private IEnumerator WaitTimeGoToSceneGame(ScreenFader screenFaderInCorutine)
    {
        screenFaderInCorutine.fadeSpeed = 2f;
        screenFaderInCorutine.fadeState = ScreenFader.FadeState.In;
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("GameZone");
    }

    //click restart game when game over to start new game
    public void Restart()
    {
        DataAd dataAd = LoadSaveAd.Load();
        if (!dataAd.isRatedApp)
        {
            dataAd.clicksOnRestartButton++;
            LoadSaveAd.Save(dataAd);
        }

        SceneManager.LoadScene("GameZone");
    }

    public void PauseNo()
    {
        GameObject.Find("PauseWindow").SetActive(false);

        RunGame.isPause = false;

        Time.timeScale = 1;
    }

    public void PauseYes()
    {
        Application.Quit();
    }

    public void PauseReturnToMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("MainMenu");
    }

    //show message that sat that this feature will be coming soon (only in main menu)
    public void msgComingSoon()
    {
        GameObject.Find("CanvasClassic").transform.Find("ComingSoon").gameObject.SetActive(true);
        StartCoroutine(Wait(1, () => {
            GameObject.Find("CanvasClassic").transform.Find("ComingSoon").gameObject.SetActive(false);
        }));
    }

    public void msgInformation()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowInformation").gameObject.SetActive(true);
        StartCoroutine(Wait(2, () => {
            GameObject.Find("CanvasClassic").transform.Find("WindowInformation").gameObject.SetActive(false);
        }));
    }

    /// <summary>
    /// return to main menu after shop
    /// </summary>
    public void ExitShop()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").gameObject.SetActive(false);
    }

    /// <summary>
    /// return to main menu after profile
    /// </summary>
    public void ExitProfile()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowProfile").gameObject.SetActive(false);
    }

    public void ExitQuests()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").gameObject.SetActive(false);
    }

    public void ExitPanelQuests()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
            Find("PanelBlack").gameObject.SetActive(false);
    }
    /// <summary>
    /// function starts work when toggle send signal
    /// </summary>
    /// <param name="i">say witch animals sent a signal</param>
    public void ToggleAnimalInShop(int i)
    {       
        //get an animal in shop
        GameObject anAnimal = GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find(i.ToString()).gameObject;

        //hide buy button
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find("BuyButton").gameObject.SetActive(false);

        //get component toggle of an animal
        Toggle toggle = anAnimal.GetComponent<Toggle>();
        
        if (toggle.isOn)
        {
            toggle.interactable = false;

            //costumer can buy the animal
            if (animals.CostAnimals[i] <= DataplayerManager.instance.Data.coins)
            {
                //show buy button
                GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find("BuyButton").gameObject.SetActive(true);
            }

        }
        else if (!DataplayerManager.instance.Data.boughtAnimals.Contains(i))
        {
            //do that can't click on toggle
            toggle.interactable = true;
        }

        

    }

    /// <summary>
    /// doing monipulations when buy button was clicked
    /// </summary>
    public void BuyButton()
    {
        for (int i = 0; i != 3; i++)
        {
            //find an animal
            GameObject anAnimal = GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find(i.ToString()).gameObject;

            //check witch an animal costumer want to buy
            if (anAnimal.GetComponent<Toggle>().isOn)
            {
                DataplayerManager.instance.Data.coins -= animals.CostAnimals[i];
                //add animal to bought animal list
                DataplayerManager.instance.Data.boughtAnimals.Add(i);
                //add to current animal
                DataplayerManager.instance.Data.currentAnimal = i;

                //save data
                //LoadSavePlayer.Save(BackgroundMenu.dataPlayer);
                DataplayerManager.instance.Save();

                //update menu values
                BackgroundMenu.setValuesInStart(animals);

                GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find("BuyButton").gameObject.SetActive(false);

            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i">id of animal</param>
    public void ToggleAnimalInProfile(int i)
    {
        //find an animal in canvas classic
        GameObject anAnimal = GameObject.Find("CanvasClassic").transform.Find("WindowProfile").
        Find("SectionAnimals").Find(i.ToString()).gameObject;

        //get toggle component of animal
        Toggle toggle = anAnimal.GetComponent<Toggle>();

        if (toggle.isOn)
        {
            //for can't click
            toggle.interactable = false;

            //show vi
            anAnimal.transform.Find("Vi").gameObject.SetActive(true);

            //set new current animal
            DataplayerManager.instance.Data.currentAnimal = i;
            //save data
            //LoadSavePlayer.Save(BackgroundMenu.dataPlayer);
            DataplayerManager.instance.Save();

        }
        else
        {
            //hide vi
            anAnimal.transform.Find("Vi").gameObject.SetActive(false);
            //for can click
            toggle.interactable = true;
        }
    }
        

    #region buttons for test game
    public void MoreMoney()
    {
        DataplayerManager.instance.Data.coins += 1000;
        //LoadSavePlayer.Save(BackgroundMenu.dataPlayer);
        DataplayerManager.instance.Save();
        //update menu
        BackgroundMenu.setValuesInStart(animals);
    }

    public void DefDat()
    {
        DataplayerManager.instance.Data.SetDefoultData();
        //LoadSavePlayer.Save(BackgroundMenu.dataPlayer);
        DataplayerManager.instance.Save();

        DataQuests data = LoadSaveQuests.Load();
        data.SetDefoultData();
        LoadSaveQuests.Save(data);

        DataAd dataAd = LoadSaveAd.Load();
        dataAd.SetDefoultData();
        LoadSaveAd.Save(dataAd);

        //update menu
        BackgroundMenu.setValuesInStart(animals);


    }
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

