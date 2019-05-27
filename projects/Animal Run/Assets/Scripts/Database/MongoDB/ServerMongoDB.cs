using UnityEngine;
using System; //
using System.Collections;
using System.Collections.Generic;  // Lists

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

public class ServerMongoDB : MonoBehaviour, IDatabaseable
{
	private string _connectionString = "mongodb://localhost:27017";
	public MongoDatabase _database = null;

	public void RunServer()
	{
		// Start server
		_database = new MongoClient(_connectionString).GetServer().GetDatabase("catstripdb");
		Debug.Log("ESTABLISHED CONNECTION TO MONGODB");
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

		if(data.BoughtAnimals != null)
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

	/// <summary>
	/// Check if the values in database are contains and they are correct.
	/// </summary>
	private void CheckDatabase()
	{
		// Check if there are bought animal
		var animalInUserCollection = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER");
		var userCollection = _database.GetCollection<BsonDocument>("USER");
		ObjectId idUser = userCollection.FindOne()["_id"].AsObjectId;
		if (animalInUserCollection.Count() == 0)
		{
			Animal animalString = (Animal)0;
			var an = _database.GetCollection<BsonDocument>("ANIMALS").
				Find(new QueryDocument("name", animalString.ToString()));
			ObjectId idAnimal = new ObjectId();
			foreach (var document in an)
			{
				idAnimal = document["_id"].AsObjectId;
			}

			animalInUserCollection.Insert(new BsonDocument
			{
				{ "animal_id", idAnimal },
				{ "user_id",  idUser},
				{ "current", true }
			});
		}

		// Check if there are current animal
		if(GetCurrentAnimal() == -1)
		{
			DataPlayer data = new DataPlayer();
			data.SetDefoultData();
			SetCurrentAnimal(data);
		}

	}

	private void SetCurrentAnimal(DataPlayer data)
	{
		var animalInUserCollection = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER");

		// Set false to old current animal
		var where14 = new QueryDocument{
			{"current", true}
		};
		var set14 = new UpdateDocument {
			{ "$set", new BsonDocument ("current", false) }
		};
		animalInUserCollection.Update(where14, set14);

		// Set true to new animal
		Animal an = (Animal)data.CurrentAnimal;
		var currentAnimal = _database.GetCollection<BsonDocument>("ANIMALS").
	Find(new QueryDocument("name", an.ToString()));

		ObjectId id = new ObjectId();
		foreach (var document in currentAnimal)
		{
			id = document["_id"].AsObjectId;
		}

		var where5 = new QueryDocument{
			{"animal_id", id}
		};
		var set5 = new UpdateDocument {
			{ "$set", new BsonDocument ("current", true) }
		};
		animalInUserCollection.Update(where5, set5);
	}

	private void SetBoughtAnimals(DataPlayer data)
	{
		CheckDatabase();

		int current = GetCurrentAnimal();
		var userAnimalCollection = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER");

		// Remove all bought animals.
		userAnimalCollection.RemoveAll();

		// Add the new list of bought animals.
		var userCollection = _database.GetCollection<BsonDocument>("USER");
		ObjectId idUser = userCollection.FindOne()["_id"].AsObjectId;
		var animalCollection = _database.GetCollection<BsonDocument>("ANIMALS");

		foreach (var animal in data.BoughtAnimals)
		{
			Animal animalString = (Animal)animal;
			var an = animalCollection.Find(new QueryDocument("name", animalString.ToString()));

			bool isCurrent = false;
			// Set ass current animal
			if(animal == current)
			{
				isCurrent = true;
			}

			ObjectId idAnimal = new ObjectId();
			foreach (var document in an)
			{
				idAnimal = document["_id"].AsObjectId;
			}
			userAnimalCollection.Insert(new BsonDocument{
			{ "animal_id", idAnimal },
			{ "user_id",  idUser},
			{ "current", isCurrent }
		});

		}
	}

	private void SetCoins(DataPlayer data)
	{
		var userCollection = _database.GetCollection<BsonDocument>("USER");

		var where14 = new QueryDocument{
			{"_id", userCollection.FindOne()["_id"]}
		};
		var set14 = new UpdateDocument {
			{ "$set", new BsonDocument ("money", data.Coins) }
		};
		userCollection.Update(where14, set14);
	}

	private void SetScore(DataPlayer data)
	{
		var userCollection = _database.GetCollection<BsonDocument>("USER");

		var where14 = new QueryDocument{
			{"_id", userCollection.FindOne()["_id"]}
		};
		var set14 = new UpdateDocument {
			{ "$set", new BsonDocument ("score", data.Score) }
		};
		userCollection.Update(where14, set14);
	}

	private List<int> GetBoughtAnimals()
	{
		List<int> boughtAnimals = new List<int>();

		var animals = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER").FindAll();
		ObjectId id = new ObjectId();
		foreach (var document in animals)
		{
			id = document["animal_id"].AsObjectId;
			var animal = _database.GetCollection<BsonDocument>("ANIMALS").
			Find(new QueryDocument("_id", id));
			string nameAnimal = "";
			foreach (var item in animal)
			{
				nameAnimal = item["name"].AsString;
			}
			Debug.Log(nameAnimal);
			boughtAnimals.Add((int)Enum.Parse(typeof(Animal), nameAnimal));
		}

		return boughtAnimals;
	}

	private int GetCoins()
	{
		return _database.GetCollection<BsonDocument>("USER").
			FindOne()["money"].AsInt32;
	}

	private int GetCurrentAnimal()
	{
		var currentAnimal = _database.GetCollection<BsonDocument>("ANIMAL_IN_USER").
			Find(new QueryDocument("current", true));

		ObjectId id = new ObjectId();
		foreach (var document in currentAnimal)
		{
			id = document["animal_id"].AsObjectId;
		}

		var animal = _database.GetCollection<BsonDocument>("ANIMALS").
			Find(new QueryDocument("_id", id));

		string nameAnimal = "";
		foreach (var item in animal)
		{
			nameAnimal = item["name"].AsString;
		}
		Debug.Log(nameAnimal);
		if (nameAnimal == "")
		{
			// return error
			return -1;
		}
		return (int)Enum.Parse(typeof(Animal), nameAnimal);
	}

	private int GetScore()
	{
		return _database.GetCollection<BsonDocument>("USER").
			FindOne()["score"].AsInt32;
	}

}
