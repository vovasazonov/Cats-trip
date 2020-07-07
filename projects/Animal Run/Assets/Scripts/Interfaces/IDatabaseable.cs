using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatabaseable
{
	void RunServer();

	/// <summary>
	/// Load data from data base and set to DataPlayer.
	/// </summary>
	DataPlayer LoadData();
	/// <summary>
	/// Save data from dataplayer to database.
	/// </summary>
	void SaveData(DataPlayer data);

	// User
	//int GetCoins();
	//int GetScore();
	//int GetLevel();
	//double GetExp();
	//bool GetProAccount();

	//void SetCoins();
	//void SetScore();
	//void SetLevel();
	//double SetExp();
	//bool SetProAccount();
}
