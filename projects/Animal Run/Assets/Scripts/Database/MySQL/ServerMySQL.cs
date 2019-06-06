using System.Collections.Generic;  // Lists

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
//using MySql.Data;
//using MySql.Data.MySqlClient;

public class ServerMySQL : MonoBehaviour, IDatabaseable
{
	//MySqlConnection connection;
	//private string _connectionString = "SERVER=25.58.57.135;" + "DATABASE=cats_trip;" + "UID=user;" + "PASSWORD=12345;";

	//public void Start()
	//{
	//	RunServer();

	//	DataPlayer data = new DataPlayer();
	//	data.Coins = 1234;
	//	data.Score = 9876;
	//	data.CurrentAnimal = 2;

	//	SetCoins(data);
	//	SetScore(data);
	//	SetCurrentAnimal(data);
	//	Debug.Log("Money = " + GetCoins());
	//	Debug.Log("Score = " + GetScore());
	//	Debug.Log("Current animal = " + GetCurrentAnimal());
	//}

	public void RunServer()
	{
		//// Start server
		//if (connection == null)
		//{
		//	try
		//	{
		//		connection = new MySqlConnection(_connectionString);
		//		connection.Open();
		//	}
		//	catch (MySqlException ex)
		//	{
		//		Debug.LogError("MySQL Error: " + ex.ToString());
		//	}
		//}
		//Debug.Log("ESTABLISHED CONNECTION TO MySQL");
	}

	public DataPlayer LoadData()
	{
		return null;
	}

	public void SaveData(DataPlayer data)
	{

	}

	//private int GetCoins()

	//{
	//	int tempCoins = 0;
	//	if (connection != null)
	//	{
	//		string sqlCoins = "SELECT money FROM user WHERE user_id = 13";
	//		MySqlCommand command = new MySqlCommand(sqlCoins, connection);
	//		string name = command.ExecuteScalar().ToString();

	//		int.TryParse(name, out tempCoins);
	//	}
	//	return tempCoins;
	//}

	//private void SetCoins(DataPlayer data)
	//{
	//	int dataCoins = data.Coins;
	//	{
	//		string reqCoins = "UPDATE user SET money = " + dataCoins.ToString() + " WHERE user_id = 13";
	//		MySqlCommand command = new MySqlCommand(reqCoins, connection);
	//		command.ExecuteScalar();
	//	}
	//}

	//private int GetScore()
	//{
	//	int tempScore = 0;
	//	if (connection != null)
	//	{
	//		string sqlScore = "SELECT score FROM user WHERE user_id = 13";
	//		MySqlCommand command = new MySqlCommand(sqlScore, connection);
	//		string name = command.ExecuteScalar().ToString();

	//		int.TryParse(name, out tempScore);
	//	}
	//	return tempScore;
	//}

	//private void SetScore(DataPlayer data)
	//{
	//	int dataScore = data.Score;
	//	{
	//		string reqCoins = "UPDATE user SET score = " + dataScore.ToString() + " WHERE user_id = 13";
	//		MySqlCommand command = new MySqlCommand(reqCoins, connection);
	//		command.ExecuteScalar();
	//	}
	//}

	//private string GetCurrentAnimal()
	//{
	//	string name = "";
	//	if (connection != null)
	//	{
	//		string sqlCurrAnimalqTemp = "SELECT animal_id FROM animal_in_user WHERE current = 1";
	//		MySqlCommand commandTemp = new MySqlCommand(sqlCurrAnimalqTemp, connection);
	//		string nameTemp = commandTemp.ExecuteScalar().ToString();

	//		string sqlCurrAnimalq = "SELECT name FROM animals WHERE animal_id = " + nameTemp;
	//		MySqlCommand command = new MySqlCommand(sqlCurrAnimalq, connection);
	//		name = command.ExecuteScalar().ToString();
	//	}

	//	return name;
	//}

	//private void SetCurrentAnimal(DataPlayer data)
	//{
	//	int dataSetCurrAnimalTemp = data.CurrentAnimal;
	//	{
	//		string reqCurrAnimalTemp = "UPDATE animal_in_user SET current = 0 WHERE current = 1";
	//		MySqlCommand commandTemp = new MySqlCommand(reqCurrAnimalTemp, connection);
	//		commandTemp.ExecuteScalar();

	//		string reqCurrAnimal = "UPDATE animal_in_user SET current = 1 WHERE animal_id = " + dataSetCurrAnimalTemp.ToString();
	//		MySqlCommand command = new MySqlCommand(reqCurrAnimal, connection);
	//		command.ExecuteScalar();
	//	}
	//}
	//-------------------------------+++++++++++++++++++++++++++++++----------------------

	// public DataPlayer LoadData()
	// {
	// 	CheckDatabase();

	// 	DataPlayer data = new DataPlayer();

	// 	//Комментируем все, кроме нужного
	// 	data.BoughtAnimals = GetBoughtAnimals();
	// 	Debug.Log(data.BoughtAnimals);

	// 	data.Coins = GetCoins();
	// 	Debug.Log(data.Coins);

	// 	data.CurrentAnimal = GetCurrentAnimal();
	// 	Debug.Log(data.BoughtAnimals);

	// 	data.Score = GetScore();
	// 	Debug.Log(data.Score);

	// 	return data;
	// }

	// public void SaveData(DataPlayer data)
	// {
	// 	CheckDatabase();

	// 	if(data.BoughtAnimals != null)
	// 	{
	// 		//data.BoughtAnimals
	// 		SetBoughtAnimals(data);
	// 		SetCurrentAnimal(data);
	// 	}

	// 	//data.Coins
	// 	SetCoins(data);

	// 	//data.Score
	// 	SetScore(data);
	// }

	/// <summary>
	/// Check if the values in database are contains and they are correct.
	/// </summary>
	// private void CheckDatabase()
	// {
	// 	// Check if there are bought animal
	// 	var animalInUserCollection = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER");
	// 	var userCollection = _database.GetCollection<BsonDocument>("USER");
	// 	ObjectId idUser = userCollection.FindOne()["_id"].AsObjectId;
	// 	if (animalInUserCollection.Count() == 0)
	// 	{
	// 		Animal animalString = (Animal)0;
	// 		var an = _database.GetCollection<BsonDocument>("ANIMALS").
	// 			Find(new QueryDocument("name", animalString.ToString()));
	// 		ObjectId idAnimal = new ObjectId();
	// 		foreach (var document in an)
	// 		{
	// 			idAnimal = document["_id"].AsObjectId;
	// 		}

	// 		animalInUserCollection.Insert(new BsonDocument
	// 		{
	// 			{ "animal_id", idAnimal },
	// 			{ "user_id",  idUser},
	// 			{ "current", true }
	// 		});
	// 	}

	// 	// Check if there are current animal
	// 	if(GetCurrentAnimal() == -1)
	// 	{
	// 		DataPlayer data = new DataPlayer();
	// 		data.SetDefoultData();
	// 		SetCurrentAnimal(data);
	// 	}

	// }



	// private void SetBoughtAnimals(DataPlayer data)
	// {
	// 	CheckDatabase();

	// 	int current = GetCurrentAnimal();
	// 	var userAnimalCollection = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER");

	// 	// Remove all bought animals.
	// 	userAnimalCollection.RemoveAll();

	// 	// Add the new list of bought animals.
	// 	var userCollection = _database.GetCollection<BsonDocument>("USER");
	// 	ObjectId idUser = userCollection.FindOne()["_id"].AsObjectId;
	// 	var animalCollection = _database.GetCollection<BsonDocument>("ANIMALS");

	// 	foreach (var animal in data.BoughtAnimals)
	// 	{
	// 		Animal animalString = (Animal)animal;
	// 		var an = animalCollection.Find(new QueryDocument("name", animalString.ToString()));

	// 		bool isCurrent = false;
	// 		// Set ass current animal
	// 		if(animal == current)
	// 		{
	// 			isCurrent = true;
	// 		}

	// 		ObjectId idAnimal = new ObjectId();
	// 		foreach (var document in an)
	// 		{
	// 			idAnimal = document["_id"].AsObjectId;
	// 		}
	// 		userAnimalCollection.Insert(new BsonDocument{
	// 		{ "animal_id", idAnimal },
	// 		{ "user_id",  idUser},
	// 		{ "current", isCurrent }
	// 	});

	// 	}
	// }


	// private List<int> GetBoughtAnimals()
	// {
	// 	List<int> boughtAnimals = new List<int>();

	// 	var animals = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER").FindAll();
	// 	ObjectId id = new ObjectId();
	// 	foreach (var document in animals)
	// 	{
	// 		id = document["animal_id"].AsObjectId;
	// 		var animal = _database.GetCollection<BsonDocument>("ANIMALS").
	// 		Find(new QueryDocument("_id", id));
	// 		string nameAnimal = "";
	// 		foreach (var item in animal)
	// 		{
	// 			nameAnimal = item["name"].AsString;
	// 		}
	// 		Debug.Log(nameAnimal);
	// 		boughtAnimals.Add((int)Enum.Parse(typeof(Animal), nameAnimal));
	// 	}

	// 	return boughtAnimals;
	// }

}
