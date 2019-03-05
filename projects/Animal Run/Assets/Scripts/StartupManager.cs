using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    //WARNING! THE SCRIPT MUST BE IN LOAD SCENE, EITHER, IN THE SCENE MAINMENU WILL BE A LOOP

    // Use this for initialization
    private IEnumerator Start()
    {
        //get default languege english (Поменять это, чтобы в файле данных по умолчанию был английский но если вдруг изменил, то 
        //загружать уже другой язык а пока пусть будет английский для тесте
        LocalizationManager.instance.LoadLocalizedText("localizedText_en.json");
        //load DataPlayer (coins, bought animals, etc...)
        DataplayerManager.instance.Load();
        //load AdData
        AdManager.instance.Load();
        //load dataQuests
        QuestsManager.instance.Load();

        while (!LocalizationManager.instance.IsReady && !DataplayerManager.instance.IsReady &&
            !AdManager.instance.IsReady && !QuestsManager.instance.IsReady)
        {
            yield return null;
        }

        //load scen when all data loaded
        SceneManager.LoadScene("MainMenu");
    }

}