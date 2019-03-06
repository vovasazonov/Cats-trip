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

	private static StartupManager _instance;

    // Use this for initialization
    void Start()
    {
		IData iData;

		// There are not object yet.
		// Data didnt loaded yet.
		if (_instance == null)
		{
			_instance = this;
			
			// Load DataPlayer
			iData = (IData)DataplayerManager.Instance.Data;
			LoadSave.Load(ref iData, DataplayerManager.Instance.NameFile);
			DataplayerManager.Instance.Data = (DataPlayer)iData;
			// Load AdData
			iData = (IData)AdManager.Instance.Data;
			LoadSave.Load(ref iData, AdManager.Instance.NameFile);
			AdManager.Instance.Data = (DataAd)iData;
			// Load dataQuests
			iData = (IData)QuestsManager.instance.Data;
			LoadSave.Load(ref iData, QuestsManager.instance.NameFile);
			QuestsManager.instance = (QuestsManager)iData;

			// Get languege
			/* Change to next alghorithm:
			 * if not changing language by user
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
}