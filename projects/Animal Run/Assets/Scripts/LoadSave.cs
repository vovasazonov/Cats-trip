/*
*	Copyright (c) NromaGames
*	Developer Sazonov Vladimir (Emilio) 
*	Email : futureNroma@yandex.ru
*/

using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Load and save data to file. 
/// Need only two arguments: IData and nameFile.
/// </summary>
static public class LoadSave {

	/// <summary>
	/// Save the class with data information interaction.
	/// </summary>
	static public void Save<T>(T Data, string nameFile) where T : IData
	{
		BinaryFormatter bf = new BinaryFormatter();

		// Check if folder "saves" exists.
		if (!Directory.Exists(Application.persistentDataPath + "/saves"))
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");

		// Create new file.
		FileStream file = new FileStream(Application.persistentDataPath +
			"/saves/"+ nameFile, FileMode.Create);

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
	/// <returns>Is loaded</returns>
	static public void Load<T>(ref T Data, string nameFile) where T : IData
	{
		// Check if folder "saves" exists.
		if (File.Exists(Application.persistentDataPath + "/saves/"+ nameFile))
		{
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file = File.Open(Application.persistentDataPath + "/saves/"+ 
				nameFile, FileMode.Open);

			try
			{
				// Load data.
				Data = (T)bf.Deserialize(file);
				file.Close();
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);

				// Close file
				file.Close();

				// Set defoult values
				Data.SetDefoultData();

				// Save new data
				Save(Data, nameFile);
			}
		}
		else
		{
			// Set defoult values
			Data.SetDefoultData();

			// Save new data
			Save(Data, nameFile);
		}
	}
}
