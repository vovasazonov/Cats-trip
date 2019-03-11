/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

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

	/// <summary>
	/// Call when user click on later to rate application
	/// in google play.
	/// </summary>
    public void ClickLaterRateButton()
    {
		DataAd data = AdManager.Instance.Data;

        data.ClicksOnRestartButton = 0;

		// Save
		LoadSave.Save(data, AdManager.Instance.NameFile);

        // Hide rate window
        GameObject.Find("CanvasClassic").transform.
            Find("WindowRate").gameObject.SetActive(false);
    }

	/// <summary>
	/// Call when user click to rate application
	/// in google play.
	/// </summary>
    public void ClickRateButton()
    {
		DataAd data = AdManager.Instance.Data;

		data.ClicksOnRestartButton = 0;
        data.IsRatedApp = true;

		// Save
		LoadSave.Save(data, AdManager.Instance.NameFile);

		// Hide rate window
		GameObject.Find("CanvasClassic").transform.
            Find("WindowRate").gameObject.SetActive(false);
        // Open play market to rate my app
        Application.OpenURL("market://details?id=com.Nroma.Catstrip");
    }

    /// <summary>
    /// Call when user click on play button.
	/// It's start a game.
    /// </summary>
    public void ClickPlayButton()
    {
        screenFader = GetComponent<ScreenFader>();   
		// Open scene game
        StartCoroutine(WaitTimeGoToSceneGame(screenFader));   
    }

	/// <summary>
	/// Call when click on shop button in menu
	/// </summary>
    public void ClickShopButton()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").gameObject.SetActive(true);
    }
	/// <summary>
	/// Call when click on profile button
	/// </summary>
	public void ClickProfileButton()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowProfile").gameObject.SetActive(true);
    }
	/// <summary>
	/// Call when click on quest button in menu
	/// </summary>
	public void ClickQuestsButton()
    {
		DataQuests dataQuests = QuestsManager.Instance.Data;

        // Get button video rewarded
        GameObject buttonRewardVideo = GameObject.Find("CanvasClassic").transform.
            Find("WindowQuests").Find("ButtonRewardVideo").gameObject;

        if (dataQuests.DateQuest != DateTime.Today)
        {
            dataQuests.SetDefoultData();
			LoadSave.Save(dataQuests, QuestsManager.Instance.NameFile);
        }

        if(dataQuests.WasTodayRewardVideo)
        {
            buttonRewardVideo.SetActive(false);

            // Show text that there are not rewarded
            GameObject.Find("CanvasClassic").transform.
                Find("WindowQuests").Find("NoGiftsText").gameObject.SetActive(true);
        }
        else
        {
            buttonRewardVideo.SetActive(true);
        }

        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").gameObject.SetActive(true);
    }

	/// <summary>
	/// Call when click music button
	/// </summary>
	public void ClickMusicButton(GameObject buttonMusic)
    {
        Animals animals = new Animals();
        DataPlayer dataPlayer = DataplayerManager.Instance.Data;

        if(buttonMusic.GetComponent<Toggle>().isOn == true)
        {
            dataPlayer.IsMusicMainMenu = true;
        }
        else
        {
            dataPlayer.IsMusicMainMenu = false;
        }

		LoadSave.Save(dataPlayer, DataplayerManager.Instance.NameFile);

        BackgroundMenu.SetValuesInStart(animals);
    }

	/// <summary>
	/// Coroutin that make effect of shadow 
	/// and after this start scene GameZone
	/// </summary>
	/// <param name="screenFaderInCorutine"></param>
	/// <returns></returns>
	private IEnumerator WaitTimeGoToSceneGame(ScreenFader screenFaderInCorutine)
    {
        screenFaderInCorutine.fadeSpeed = 2f;
        screenFaderInCorutine.fadeState = ScreenFader.FadeState.In;
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("GameZone");
    }

	/// <summary>
	/// Call when click restart game button
	/// when game over, to start new game.
	/// </summary>
	public void Restart()
    {
		DataAd dataAd = AdManager.Instance.Data;

        if (!dataAd.IsRatedApp)
        {
            dataAd.ClicksOnRestartButton++;
			LoadSave.Save(dataAd, AdManager.Instance.NameFile);
        }

        SceneManager.LoadScene("GameZone");
    }

	/// <summary>
	/// Call when click on button No on pause window.
	/// Continue the game.
	/// </summary>
    public void PauseNo()
    {
        GameObject.Find("PauseWindow").SetActive(false);

        RunGame.isPause = false;

        Time.timeScale = 1;
    }

	/// <summary>
	/// Call when click on button Yes on pause window.
	/// Out of the application.
	/// </summary>
	public void PauseYes()
    {
        Application.Quit();
    }

	/// <summary>
	/// Call when click on button Return To Menu on pause window.
	/// Return to main menu.
	/// </summary>
	public void PauseReturnToMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("MainMenu");
    }

	/// <summary>
	/// Show message that sat that this feature will be coming soon (only in main menu work!)
	/// </summary>
	public void ShowMsgComingSoon()
    {
        GameObject.Find("CanvasClassic").transform.Find("ComingSoon").gameObject.SetActive(true);
        StartCoroutine(Wait(1, () => {
            GameObject.Find("CanvasClassic").transform.Find("ComingSoon").gameObject.SetActive(false);
        }));
    }

	/// <summary>
	/// Show information window (only in main menu work!)
	/// </summary>
	public void ShowMsgInformation()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowInformation").gameObject.SetActive(true);
        StartCoroutine(Wait(2, () => {
            GameObject.Find("CanvasClassic").transform.Find("WindowInformation").gameObject.SetActive(false);
        }));
    }

    /// <summary>
    /// Hide shop window.
    /// </summary>
    public void HideShop()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").gameObject.SetActive(false);
    }

    /// <summary>
    /// Hide profile window
    /// </summary>
    public void HideProfile()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowProfile").gameObject.SetActive(false);
    }

	/// <summary>
	/// Hide quests window
	/// </summary>
	public void HideQuests()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").gameObject.SetActive(false);
    }

	/// <summary>
	/// Hide panel quests
	/// </summary>
	public void HideBlackPanelQuests()
    {
        GameObject.Find("CanvasClassic").transform.Find("WindowQuests").
            Find("PanelBlack").gameObject.SetActive(false);
    }
    /// <summary>
    /// Call toggle send signal in shop
    /// </summary>
    /// <param name="i">say witch animals sent a signal</param>
    public void ToggleAnimalInShop(int i)
    {       
        // Get an animal in shop
        GameObject anAnimal = GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find(i.ToString()).gameObject;

        // Hide buy button
        GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find("BuyButton").gameObject.SetActive(false);

        // Get component toggle of an animal
        Toggle toggle = anAnimal.GetComponent<Toggle>();
        
        if (toggle.isOn)
        {
            toggle.interactable = false;

            // Costumer can buy the animal
            if (animals.CostAnimals[i] <= DataplayerManager.Instance.Data.Coins)
            {
                // Show buy button
                GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find("BuyButton").gameObject.SetActive(true);
            }

        }
        else if (!DataplayerManager.Instance.Data.BoughtAnimals.Contains(i))
        {
            // Do that can't click on toggle
            toggle.interactable = true;
        }
    }

    /// <summary>
    /// Call when clicked on buy button.
    /// </summary>
    public void BuyButton()
    {
        for (int i = 0; i != 3; i++)
        {
            // Find an animal
            GameObject anAnimal = GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find(i.ToString()).gameObject;

            // Check witch an animal costumer want to buy
            if (anAnimal.GetComponent<Toggle>().isOn)
            {
                DataplayerManager.Instance.Data.Coins -= animals.CostAnimals[i];
                // Add animal to bought animal list
                DataplayerManager.Instance.Data.BoughtAnimals.Add(i);
                // Add to current animal
                DataplayerManager.Instance.Data.CurrentAnimal = i;

				// Save data
                LoadSave.Save(DataplayerManager.Instance.Data,
					DataplayerManager.Instance.NameFile);

                // Update menu values
                BackgroundMenu.SetValuesInStart(animals);

                GameObject.Find("CanvasClassic").transform.Find("WindowShop").
            Find("SectionAnimals").Find("BuyButton").gameObject.SetActive(false);

            }
        }
    }

    /// <summary>
    /// Call when user choosed animal to play with it
	/// on profile window (choose from bought animals).
    /// </summary>
    /// <param name="i">Id of animal</param>
    public void ToggleAnimalInProfile(int i)
    {
        // Find an animal in canvas classic
        GameObject anAnimal = GameObject.Find("CanvasClassic").transform.Find("WindowProfile").
        Find("SectionAnimals").Find(i.ToString()).gameObject;

        // Get toggle component of animal
        Toggle toggle = anAnimal.GetComponent<Toggle>();

        if (toggle.isOn)
        {
            // Set for user unclickable on current animal (he choosed it).
            toggle.interactable = false;

			// Show vi on animal that choosed
			anAnimal.transform.Find("Vi").gameObject.SetActive(true);

            // Set new current animal
            DataplayerManager.Instance.Data.CurrentAnimal = i;
			
			// Save data.
			LoadSave.Save(DataplayerManager.Instance.Data,
				DataplayerManager.Instance.NameFile);
		}
        else
        {
			// Hide vi from animal that didnt choosed.
			anAnimal.transform.Find("Vi").gameObject.SetActive(false);
			// Set for user clickable on animal that didnt choosed.
			toggle.interactable = true;
        }
    }
        

    #region buttons for test game
    public void MoreMoney()
    {
        DataplayerManager.Instance.Data.Coins += 1000;
		//LoadSavePlayer.Save(BackgroundMenu.dataPlayer);
		LoadSave.Save(DataplayerManager.Instance.Data, DataplayerManager.Instance.NameFile);
		//update menu
		BackgroundMenu.SetValuesInStart(animals);
    }

	/// <summary>
	/// Set default data
	/// </summary>
    public void DefDat()
    {
        DataPlayer dataPlayer = DataplayerManager.Instance.Data;
		dataPlayer.SetDefoultData();
		LoadSave.Save(dataPlayer, DataplayerManager.Instance.NameFile);

		DataQuests dataQuests = QuestsManager.Instance.Data;
        dataQuests.SetDefoultData();
		LoadSave.Save(dataQuests, QuestsManager.Instance.NameFile);

		DataAd dataAd = AdManager.Instance.Data;
        dataAd.SetDefoultData();
		LoadSave.Save(dataAd, AdManager.Instance.NameFile);

        //Update menu
        BackgroundMenu.SetValuesInStart(animals);
    }
    #endregion

    delegate void SomeMethod();
    /// <summary>
    /// Wait some time before start some method
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

