using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class DataplayerManager : MonoBehaviour {

	#region Variables
    public static DataplayerManager instance;

	// Data that keeping information.
	public DataPlayer Data { get; set; }
	// True if data loaded.
	public bool IsReady { get; private set; }

	// Name of file.
	private string _nameFile = "playerInfo.dat";
	#endregion


	/// load the class DataPlayer with information of customer
	public void Load()
    {
		IsReady = false;
        //Data = LoadSavePlayer.Load();
        //isReady = true;

        //check if folder "saves" exists
        if (File.Exists(Application.persistentDataPath + "/saves/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/saves/playerInfo.dat", FileMode.Open);

            //try load data
            try
            {
                //load data
                Data = (DataPlayer)bf.Deserialize(file);
                file.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);

                //close load file
                file.Close();

                //set defoult values and save in new file
                Data = new DataPlayer();
                Data.SetDefoultData();

                //save new data
                Save();

                //go to mainMenu
                SceneManager.LoadScene("LoadCatsTrip");
            }
        }
        else
        {
            //set defoult values and save in new file
            Data.SetDefoultData();
            Save();

            //go to mainMenu
            SceneManager.LoadScene("LoadCatsTrip");
        }

        IsReady = true;
    }

    /// save the class DataPlayer with information of customer
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        //check if folder "saves" exists
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");

        FileStream file = new FileStream(Application.persistentDataPath + "/saves/playerInfo.dat", FileMode.Create);

        bf.Serialize(file, Data);
        file.Close();
    }


}
