using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadSaveAd : MonoBehaviour {

    /// <summary>
    /// save the class with information of ad monipulations
    /// </summary>
    /// <param name="info">dataplayer class that will be save</param>
    public static void Save(DataAd info)
    {
        BinaryFormatter bf = new BinaryFormatter();

        //check if folder "saves" exists
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");

        FileStream file = new FileStream(Application.persistentDataPath + "/saves/adInfo.dat", FileMode.Create);

        bf.Serialize(file, info);
        file.Close();
    }
    /// <summary>
    /// load the class with information of ad
    /// </summary>
    /// <returns>data ad of customer witch will be load</returns>
    public static DataAd Load()
    {
        //class data player where will be a data
        DataAd info = new DataAd();
        
        //check if folder "saves" exists
        if (File.Exists(Application.persistentDataPath + "/saves/adInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/saves/adInfo.dat", FileMode.Open);

            //try load data
            try
            {
                //load data
                info = (DataAd)bf.Deserialize(file);
                file.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);

                //close load file
                file.Close();

                //set defoult values and save in new file
                info = new DataAd();
                info.SetDefoultData();

                //save new data
                Save(info);
            }
        }
        else
        {
            //set defoult values and save in new file
            info.SetDefoultData();
            Save(info);
        }

        return info;
    }
}
