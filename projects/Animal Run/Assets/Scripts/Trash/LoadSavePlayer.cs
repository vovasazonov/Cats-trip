using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadSavePlayer : MonoBehaviour {

    /// <summary>
    /// save the class with information of customer
    /// </summary>
    /// <param name="info">dataplayer class that will be save</param>
    public static void Save(DataPlayer info)
    {
        BinaryFormatter bf = new BinaryFormatter();

        //check if folder "saves" exists
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        
        FileStream file = new FileStream(Application.persistentDataPath + "/saves/playerInfo.dat", FileMode.Create);

        bf.Serialize(file, info);
        file.Close();
    }
    /// <summary>
    /// load the class with information of customer
    /// </summary>
    /// <returns>data player of customer witch will be load</returns>
    public static DataPlayer Load()
    {
        //class data player where will be a data
        DataPlayer info = new DataPlayer();

        //check if folder "saves" exists
        if (File.Exists(Application.persistentDataPath + "/saves/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/saves/playerInfo.dat", FileMode.Open);
            
            //try load data
            try
            {
                //load data
                info = (DataPlayer)bf.Deserialize(file);
                file.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);

                //close load file
                file.Close();
                
                //set defoult values and save in new file
                info = new DataPlayer();
                info.SetDefoultData();

                //save new data
                Save(info);

                //go to mainMenu
                SceneManager.LoadScene("LoadCatsTrip");                
            }       
        }
        else
        {
            //set defoult values and save in new file
            info.SetDefoultData();
            Save(info);

            //go to mainMenu
            SceneManager.LoadScene("LoadCatsTrip");
        }

        return info;
    }
}
