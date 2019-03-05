/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSave : MonoBehaviour {

	/// <summary>
	/// Save the class with data information interaction.
	/// </summary>
	public void Save<T>(T Data) where T : IData
	{
		BinaryFormatter bf = new BinaryFormatter();

		// Check if folder "saves" exists.
		if (!Directory.Exists(Application.persistentDataPath + "/saves"))
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");

		// Create new file.
		FileStream file = new FileStream(Application.persistentDataPath +
			"/saves/questsInfo.dat", FileMode.Create);

		bf.Serialize(file, Data);
		file.Close();
	}
	/// <summary>
	/// Load the class with information of quests monipulations.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="Data">Data class like DataAd, DataQuests end etc.</param>
	/// <param name="isReady">True if file loaded.</param>
	/// <param name="nameFile">Name of data file. </param>
	public void Load<T>(T Data, out bool isReady, string nameFile) where T : IData
	{
		isReady = false;

		//check if folder "saves" exists
		if (File.Exists(Application.persistentDataPath + "/saves/questsInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file = File.Open(Application.persistentDataPath + "/saves/questsInfo.dat", FileMode.Open);

			//try load data
			try
			{
				//load data
				Data = (DataQuests)bf.Deserialize(file);
				file.Close();

			}
			catch (Exception e)
			{
				Debug.Log(e.Message);

				//close load file
				file.Close();

				//set defoult values and save in new file
				Data = new DataQuests();
				Data.SetDefoultData();

				//save new data
				Save();
			}
		}
		else
		{
			//set defoult values and save in new file
			Data.SetDefoultData();
			Save();
		}

		IsReady = true;
	}
}
