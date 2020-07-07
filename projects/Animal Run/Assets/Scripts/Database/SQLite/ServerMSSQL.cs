using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using SQLite;

public class ServerMSSQL : MonoBehaviour, IDatabaseable
{
	private string _connectionString = "";
	public IDbConnection _database = null;

	private void Start()
	{
		_connectionString = "URI=file:" + Application.dataPath + "/Plugins/ct.s3db";
	
	}

	public void RunServer()
	{
		
	}

	public DataPlayer LoadData()
	{
		CheckDatabase();

		DataPlayer data = new DataPlayer();

		data.BoughtAnimals = GetBoughtAnimals();
		Debug.Log(data.BoughtAnimals);

		data.Coins = GetCoins();
		Debug.Log(data.Coins);

		data.CurrentAnimal = GetCurrentAnimal();
		Debug.Log(data.BoughtAnimals);

		data.Score = GetScore();
		Debug.Log(data.Score);

		return data;
	}


	public void SaveData(DataPlayer data)
	{
		CheckDatabase();

		if (data.BoughtAnimals != null)
		{
			//data.BoughtAnimals
			SetBoughtAnimals(data);
			SetCurrentAnimal(data);
		}

		//data.Coins
		SetCoins(data);

		//data.Score
		SetScore(data);
	}

	private void CheckDatabase()
	{
		// Check if there are current animal
		if (GetCurrentAnimal() == -1)
		{
			DataPlayer data = new DataPlayer();
			data.SetDefoultData();
			SetCurrentAnimal(data);
		}

		// Check if there are bought animal
		List <int> boughtAnimals = GetBoughtAnimals();
		if (boughtAnimals.Count == 0)
		{
			DataPlayer data = new DataPlayer();
			data.BoughtAnimals = new List<int>();
			data.CurrentAnimal = 0;
			SetBoughtAnimals(data);
		}
	}



	private int GetCoins()
	{
		//_database = (IDbConnection)new SqliteConnection(_connectionString);
		//_database.Open();

		//IDbCommand dbcmd = _database.CreateCommand();
		//string sqlQuery = "SELECT money FROM user";
		//dbcmd.CommandText = sqlQuery;
		//IDataReader reader = dbcmd.ExecuteReader();

		//int tempValue = 0;

		//while (reader.Read())
		//{
		//	tempValue = reader.GetInt32(0);
		//}
		//_database.Close();
		int coins = 0;
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<user> list = db.Query<user>("SELECT * FROM user");
			foreach (user item in list)
			{
				coins = item.money;
			}
			db.Close();
		}

		return coins;

		//return tempValue;
	}

	private int GetScore()
	{
		//_database = (IDbConnection)new SqliteConnection(_connectionString);
		//_database.Open();

		//IDbCommand dbcmd = _database.CreateCommand();
		//string sqlQuery = "SELECT score FROM user";
		//dbcmd.CommandText = sqlQuery;
		//IDataReader reader = dbcmd.ExecuteReader();

		//int tempValue = 0;

		//while (reader.Read())
		//{
		//	tempValue = reader.GetInt32(0);
		//}

		//_database.Close();

		int score = 0;
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<user> list = db.Query<user>("SELECT * FROM user");
			foreach (user item in list)
			{
				score = item.score;
			}
			db.Close();
		}

		return score;
	}

	private void SetCoins(DataPlayer data)
	{
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<user> list = db.Query<user>("SELECT * FROM user");
			foreach (user item in list)
			{
				item.money = data.Coins;
				db.Update(item);
			}
			db.Close();
		}
	}

	private void SetScore(DataPlayer data)
	{
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<user> list = db.Query<user>("SELECT * FROM user");
			foreach (user item in list)
			{
				item.money = data.Score;
				db.Update(item);
			}
			db.Close();
		}
	}

	private void SetCurrentAnimal(DataPlayer data)
	{
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<animal_in_user> list = db.Query<animal_in_user>("SELECT * FROM animal_in_user WHERE current != 0");
			foreach (animal_in_user item in list)
			{
				item.current = 0;

				IEnumerable<animals> animalsList = db.Query<animals>("SELECT * FROM animals");
				foreach (var thisAnimal in animalsList)
				{
					if (thisAnimal.animals_id == item.animal_id)
					{
						if (((Animal)data.CurrentAnimal).ToString() == thisAnimal.name)
						{
							item.current = 1;
						}
					}
				}

				db.Update(item);
			}
			db.Close();
		}
	}
	private int GetCurrentAnimal()
	{
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<animal_in_user> list = db.Query<animal_in_user>("SELECT * FROM animal_in_user WHERE current != 0");
			int id_animal = 0;
			foreach (animal_in_user item in list)
			{
				id_animal = item.animal_id;
			}
			IEnumerable<animals> list2 = db.Query<animals>("SELECT * FROM animals WHERE animals_id = "+id_animal);
			string nameAnimal = "";
			foreach (animals item in list2)
			{
				nameAnimal = item.name;
			}
			db.Close();
			Debug.Log(nameAnimal);
			if (nameAnimal == "")
				return -1;
			else
				return (int)Enum.Parse(typeof(Animal), nameAnimal);
		}
		return -1;
	}

	private List<int> GetBoughtAnimals()
	{
		List<int> boughtAnimals = new List<int>();
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<animal_in_user> list = db.Query<animal_in_user>("SELECT * FROM animal_in_user");
			foreach (animal_in_user item in list)
			{
				IEnumerable<animals> list2 = db.Query<animals>("SELECT * FROM animals WHERE animals_id = "+item.animal_id);
				foreach (var item2 in list2)
				{
					Debug.Log(item2.name);
					boughtAnimals.Add((int)Enum.Parse(typeof(Animal), item2.name));
				}
			}
			db.Close();
		}

		return boughtAnimals;
	}

	private void SetBoughtAnimals(DataPlayer data)
	{
		int currentAnimal = GetCurrentAnimal();

		//CheckDatabase();

		// Delete all records from table.
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			db.Execute("delete from animal_in_user");
			
			db.Close();

		}
		// Get user id
		int id_user = 0;
		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<user> list = db.Query<user>("SELECT * FROM user");
			
			foreach (user item in list)
			{
				id_user = item.id;
			}

			db.Close();
		}

		using (var db = new SQLiteConnection(Application.dataPath + "/Plugins/ct.s3db"))
		{
			IEnumerable<animals> list = db.Query<animals>("SELECT * FROM animals");

			foreach (animals item in list)
			{
				foreach (var boughtAnimal in data.BoughtAnimals)
				{
					if(item.name == ((Animal)boughtAnimal).ToString())
					{
						int isCurrent = 0;
						if (((Animal)currentAnimal).ToString() == item.name)
							isCurrent = 1;

						db.Execute("INSERT INTO animal_in_user(user_id, animal_id, current) VALUES("+ id_user+","+item.animals_id+","+isCurrent+")");
					}
				}
			}

			db.Close();
		}
	}
}
