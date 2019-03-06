/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load data in load scene and using 
/// it all active application time.
/// </summary>
public class StartupManager : MonoBehaviour
{
    //WARNING! THE SCRIPT MUST BE IN LOAD SCENE, EITHER, IN THE SCENE MAINMENU WILL BE A LOOP

    // Use this for initialization
    private void Start()
    {
		IData instance;

		// Load DataPlayer
		instance = (IData)DataplayerManager.Instance.Data;
		LoadSave.Load(ref instance, DataplayerManager.Instance.NameFile);
		DataplayerManager.Instance.Data = (DataPlayer)instance;
		// Load AdData
		instance = (IData)AdManager.Instance.Data;
		LoadSave.Load(ref instance, AdManager.Instance.NameFile);
		AdManager.Instance.Data = (DataAd)instance;
		// Load dataQuests
		instance = (IData)QuestsManager.instance.Data;
		LoadSave.Load(ref instance, QuestsManager.instance.NameFile);
		QuestsManager.instance = (QuestsManager)instance;

		// Get languege
		/* Change to next alghorithm:
		 * if not chnging language by user
		 *		try find witch languege use user and set it to default
		 *		catch set english
		 *	Now for will be always in default english
		 *	Befor change to alghorithm, be sure that font 
		 *	suppor languages
		 */
		LocalizationManager.instance.LoadLocalizedText("localizedText_en.json");

		// Load scen when all data loaded
		SceneManager.LoadScene("MainMenu");
	}
}